using server.Models.DTO;
using server.Models;

namespace server.DAL.Interface
{
    public interface ITicketDalUser
    {
        public Task<List<TicketDTOResualt>> GetInBasket(int userId);
        public Task<List<TicketDTOResualt>> GetNotInBasket(int userId);
        public Task Add(int userId, int giftId, int Amount, bool toBasket);
        public Task Remove(int userId, int giftId, int amount);
        public Task Buy(int userId, int giftId);
        public Task Buy(int userId);
    }
}
