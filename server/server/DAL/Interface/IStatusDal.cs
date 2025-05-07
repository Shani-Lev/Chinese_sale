using server.Models;
using server.Models.DTO;

namespace server.DAL.Interface
{
    public interface IStatusDal
    {
        public Task<Status> Get();
        public Task Set(SystemStatus status, DateTime? nextTime);

    }
}
