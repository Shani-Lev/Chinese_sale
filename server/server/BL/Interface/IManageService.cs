using server.Models.DTO;
using server.Models;

namespace server.BL.Interface
{
    public interface IManageService
    {
        public Task SetLottery();
        public Task<UserDTOResualt> SetLottery(int GiftId);
        public Task<List<TicketDTOm_After>> GetWinners();
        public Task<List<RevenueReport>> GetRevenue();
        public Task<Status> GetStatus();
        public Task SetStatus(DateTime nextTime, int type);
    }
}
