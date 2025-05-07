using server.Models.DTO;

namespace server.BL.Interface
{
    public interface ITicketManagerService
    {
        public Task<List<TicketDTOm_Before>> Get();
        public Task<TicketDTOm_Before> Get(int giftId);
        public Task<List<TicketDTOm_Before>> OrderByPrice();
        public Task<List<TicketDTOm_Before>> OrderBySales();
        public Task<List<UserDTOResualt>> GetUsers(int giftId);
    }
}
