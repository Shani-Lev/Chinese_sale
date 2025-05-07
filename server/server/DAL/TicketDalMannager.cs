using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Collections.Generic;

namespace server.DAL
{
    public class TicketDalMannager : ITicketDalMannager
    {
        private readonly PDbContext pDbContext;
        private readonly IMapper mapper;
        public TicketDalMannager(PDbContext pDbContext, IMapper mapper)
        {
            this.pDbContext = pDbContext;
            this.mapper = mapper;
        }

        public async Task<List<TicketDTOm_Before>> Get()
        {
            try
            {
                var tickets = await pDbContext.Tickets.Where(t => t.isInBasket != true).Include(t => t.Gift).ToListAsync();

                var ticketGroups = tickets.GroupBy(t => t.Gift).Select(
                    g => new TicketDTOm_Before()
                    {
                        Gift = mapper.Map<GiftDTOResualt>(g.Key),
                        Sales = g.Count()
                    }
                    ).ToList();

                return ticketGroups;
            }
            catch (Exception ex)
            {
                throw new Exception("Erron on getting tikets", ex);
            }
        }

        async public Task<TicketDTOm_Before> Get(int giftId)
        {
            try
            {
                var tickets = await pDbContext.Tickets
                .Include(t => t.Gift)
                .Where(t => t.GiftId == giftId)
                .Where(t => t.isInBasket != true)
                .ToListAsync();

                var ticketGroup = tickets
                    .GroupBy(t => t.Gift)
                    .Select(g => new TicketDTOm_Before
                    {
                        Gift = mapper.Map<GiftDTOResualt>(g.Key),
                        Sales = g.Count(),
                    }).FirstOrDefault(g => g.Gift.Id == giftId);

                return ticketGroup;
            }
            catch (Exception ex)
            {
                throw new Exception("Erron on getting tikets", ex);
            }
        }

        async public Task<List<TicketDTOm_Before>> OrderBySales()
        {
            List<TicketDTOm_Before> tickets = await this.Get();
            var sorted = tickets.OrderBy(t => t.Sales).Reverse().ToList();
            return sorted;
        }

        async public Task<List<TicketDTOm_Before>> OrderByPrice()
        {
            List<TicketDTOm_Before> tickets = await this.Get();
            var sorted = tickets.OrderBy(t => t.Gift.Price).ToList();
            return sorted;
        }

        async public Task<List<UserDTOResualt>> GetUsers(int giftId)
        {
            try
            {
                var tickets = await pDbContext.Tickets
                .Include(t => t.User)
                .Include(t => t.Gift)
                .Where(t => t.GiftId == giftId)
                .Where(t => t.isInBasket != true)
                .ToListAsync();

                var users = tickets
                   .Select(t => mapper.Map<UserDTOResualt>(t.User))
                   .GroupBy(u => u.Id)
                   .Select(g => g.First())
                   .ToList();

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("Erron on getting tikets", ex);
            }
        }

        async public Task RemoveAll()
        {
            try
            {
                var tickets = pDbContext.Tickets.ToListAsync();

                pDbContext.Tickets.RemoveRange(tickets.Result);

                pDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erron on dalete all tikets", ex);
            }
        }
    }
}
