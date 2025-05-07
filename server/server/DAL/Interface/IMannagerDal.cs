using server.Models;
using server.Models.DTO;

namespace server.DAL.Interface
{
    public interface IMannagerDal
    {
        public Task SetLottery();
        public Task<UserDTOResualt> SetLottery(int GiftId);
        public Task<List<TicketDTOm_After>> GetWinners();
        public Task<List<Ticket>> GetRevenue();
    }
}
