using AutoMapper;
using Microsoft.EntityFrameworkCore;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.DAL
{
    public class MannagerDal : IMannagerDal
    {

        private readonly PDbContext pDbContext;
        private readonly IMapper mapper;
        private readonly ILogger<MannagerDal> logger;
        public MannagerDal(PDbContext pDbContext, IMapper mapper, ILogger<MannagerDal> logger)
        {
            this.pDbContext = pDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        async public Task SetLottery()
        {
            var gifts = await pDbContext.Gifts.Where(g=>g.Winner == null).ToListAsync();
            foreach(var gift in gifts)
            {
                await SetLottery(gift.Id);
            }
            var tickets = await pDbContext.Tickets.Where(t => t.isInBasket == true).ToListAsync();
            pDbContext.Tickets.RemoveRange(tickets);
            await pDbContext.SaveChangesAsync();
        }

        async public Task<UserDTOResualt> SetLottery(int GiftId)
        {
            var tickets = await pDbContext.Tickets.Where(t => t.isInBasket != true).Where(g=>g.GiftId==GiftId).ToListAsync();
            if (tickets.Count == 0)
            {
                return null;
            }
            if (tickets.Count < 1)
            {
                return null;
            }
            int number = new Random().Next(tickets.Count);
            var winner = tickets[number];
            logger.LogInformation("winner: " + winner.UserId);
            tickets[number].isWin = true;
            var gift = await pDbContext.Gifts.FirstOrDefaultAsync(g => g.Id == GiftId);
            gift.UserWinnerId = winner.UserId;

            pDbContext.SaveChanges();
            return mapper.Map<UserDTOResualt>(winner.User);
        }

        async public Task<List<TicketDTOm_After>> GetWinners()
        {
            try
            {
                var tickets = await pDbContext.Tickets.Include(t=>t.User).Where(t=>t.isInBasket==false).ToListAsync();

                var ticketGroups = tickets
                    .GroupBy(t => t.GiftId)
                    
                    .Select(g => new TicketDTOm_After
                    {
                        GiftId = g.Key,
                        Sales = g.Count(),
                        winner = g.FirstOrDefault(t => t.isWin)?.User != null ? new UserDTOResualt
                        {
                            Id = g.FirstOrDefault(t => t.isWin).User.Id,
                            Phone = g.FirstOrDefault(t => t.isWin).User.Phone,
                            Email = g.FirstOrDefault(t => t.isWin).User.Email,
                            Name = g.FirstOrDefault(t => t.isWin).User.Name
                        } : null
                    }).ToList();

                return ticketGroups;
            }
            catch (Exception ex)
            {
                throw new Exception("Erron on getting tikets", ex);
            }
        }

        async public Task<List<Ticket>> GetRevenue()
        {
            try
            {
                var tickets = await pDbContext.Tickets.Include(t=>t.Gift).Where(t => t.isInBasket != true).ToListAsync();

                
                return tickets;
            }
            catch (Exception ex)
            {
                throw new Exception("Erron on getting revenue", ex);
            }
        }


    }
}


