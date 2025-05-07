using server.Models.DTO;
using server.Models;

namespace server.DAL.Interface
{
    public interface ITicketDalMannager
    {
        public Task<List<TicketDTOm_Before>> Get();
        public Task<TicketDTOm_Before> Get(int giftId);
        public Task<List<TicketDTOm_Before>> OrderByPrice();
        public Task<List<TicketDTOm_Before>> OrderBySales();
        public Task<List<UserDTOResualt>> GetUsers(int giftId);
        public Task RemoveAll();
    }
}
