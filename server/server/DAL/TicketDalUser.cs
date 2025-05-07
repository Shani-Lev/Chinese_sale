using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.DAL
{
    public class TicketDalUser : ITicketDalUser
    {
        private readonly PDbContext pDbContext;
        private readonly IMapper mapper;
        private readonly ILogger<TicketDalUser> logger;
        public TicketDalUser(PDbContext pDbContext, IMapper mapper, ILogger<TicketDalUser> logger)
        {
            this.pDbContext = pDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }
        async public Task<List<TicketDTOResualt>> GetInBasket(int userId)
        {
            try
            {
                var tickets = await pDbContext.Tickets
                    .Where(t => t.UserId == userId && t.isInBasket == true)
                    .Include(t => t.Gift)
                    .GroupBy(t => t.Gift)
                    .Select(t => new TicketDTOResualt()
                    {
                        isInBasket = true,
                        isWin = false,
                        Gift = mapper.Map<GiftDTOTheen>(t.Key),
                        Amount = t.Count()

                    })
                    .ToListAsync();
                return mapper.Map<List<TicketDTOResualt>>(tickets);
            }
            catch (Exception ex)
            {
                throw new Exception("error finding tickets");
            }
        }

        async public Task<List<TicketDTOResualt>> GetNotInBasket(int userId)
        {
            try
            {
                var tickets = await pDbContext.Tickets
    .Where(t => t.UserId == userId && t.isInBasket == false)
    .Include(t => t.Gift)
    .GroupBy(t => t.Gift)
    .ToListAsync();

                var ticketResults = new List<TicketDTOResualt>();

                foreach (var t in tickets)
                {
                    var winnerUser = await pDbContext.Tickets
                        .Where(w => w.GiftId == t.Key.Id && w.isWin == true)
                        .Include(w => w.User)
                        .Select(w => w.User)
                        .FirstOrDefaultAsync();

                    ticketResults.Add(new TicketDTOResualt()
                    {
                        isInBasket = false,
                        isWin = t.Any(s => s.isWin),
                        Gift = mapper.Map<GiftDTOTheen>(t.Key),
                        Amount = t.Count(),
                        Winner = mapper.Map<UserDTOResualt>(winnerUser) ?? null // אם לא נמצא זוכה, תן null
                    });
                }

                return ticketResults;
            }
            catch (Exception ex)
            {
                throw new Exception("error finding tickets");
            }
        }
        async public Task Add(int userId, int giftId, int Amount, bool toBasket)
        {
            try
            {
                var user = await pDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var gift = await pDbContext.Gifts.FirstOrDefaultAsync(u => u.Id == giftId);

                if (user == null || gift == null)
                {
                    throw new KeyNotFoundException("Gift or user not exsist");
                }
                for (int i = 0; i < Amount; i++)
                {
                    var ticket = new Ticket()
                    {
                        GiftId = gift.Id,
                        UserId = user.Id,
                        isInBasket = toBasket,
                        isWin = false
                    };
                    pDbContext.Tickets.AddAsync(ticket);
                }
                await pDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new Exception("Error on adding ticket" + ex.Message);
            }
        }

        async public Task Buy(int userId, int giftId)
        {
            try
            {
                var ticket = await pDbContext.Tickets.Where(t => t.UserId == userId && t.GiftId == giftId && t.isInBasket == true).ToListAsync();
                if (ticket == null)
                {
                    throw new KeyNotFoundException("Ticket not found");
                }
                for (int i = 0; i < ticket.Count; i++)
                {
                    ticket[i].isInBasket = false;
                    pDbContext.Tickets.Update(ticket[i]);
                }
                await pDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error on update ticket" + e.Message);
            }

        }
        async public Task Buy(int userId)
        {
            try
            {
                var ticket = await pDbContext.Tickets.Where(t => t.UserId == userId && t.isInBasket == true).ToListAsync();
                if (ticket == null)
                {
                    throw new KeyNotFoundException("Ticket not found");
                }
                for (int i = 0; i < ticket.Count; i++)
                {
                    ticket[i].isInBasket = false;
                    pDbContext.Tickets.Update(ticket[i]);
                }
                await pDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error on update ticket" + e.Message);
            }

        }

            async public Task Remove(int userIs, int giftId, int amount)
        {
            try
            {
                var ticket = await pDbContext.Tickets.Where(t => t.UserId == userIs && t.GiftId == giftId && t.isInBasket == true).ToListAsync();
                if (ticket == null)
                {
                    throw new KeyNotFoundException("Ticket not found");
                }
                if (ticket.Count < amount)
                {
                    throw new Exception("the amount is more then the tickets");
                }
                ticket.RemoveRange(0, ticket.Count - amount);
                pDbContext.Tickets.RemoveRange(ticket);
                await pDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error on update ticket" + e.Message);
            }
        }

    }
}
