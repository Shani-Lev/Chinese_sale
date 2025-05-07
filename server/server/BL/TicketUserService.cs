using server.BL.Interface;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.BL
{
    public class TicketUserService : ITicketUserService
    {
        private readonly ITicketDalUser ticketDal;
        public TicketUserService(ITicketDalUser ticketDal)
        {
            this.ticketDal = ticketDal;
        }

        async public Task Add(int userId, int giftId, int amount, bool toBasket)
        {
            try
            {
                await ticketDal.Add(userId, giftId, amount, toBasket);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on adding ticket");
            }
        }

        async public Task Buy(int userId, int giftId)
        {
            try
            {
                await ticketDal.Buy(userId, giftId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on updateing ticket");
            }
        }

        async public Task Buy(int userId)
        {
            try
            {
                await ticketDal.Buy(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on updateing ticket");
            }
        }

        async public Task<List<TicketDTOResualt>> GetInBasket(int userId)
        {
            try
            {
                return await ticketDal.GetInBasket(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting tickets");
            }

        }
        async public Task<List<TicketDTOResualt>> GetNotInBasket(int userId)
        {
            try
            {
                return await ticketDal.GetNotInBasket(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting tickets");
            }

        }

        async public Task Remove(int userId, int giftId, int amount)
        {
            try
            {
                await ticketDal.Remove(userId, giftId, amount);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on removing a ticket");
            }
        }
    }
}
