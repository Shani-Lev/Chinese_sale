using Microsoft.EntityFrameworkCore;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.DAL
{
    public class StatusDal : IStatusDal
    {
        private readonly PDbContext pDbContext;
        public StatusDal(PDbContext pDbContext) {
            this.pDbContext = pDbContext;
            
        }
        async public Task<Status> Get()
        {
            try
            {
                var stat = await pDbContext.Statuse.OrderByDescending(ls => ls.UpdateTo).FirstOrDefaultAsync();
                return stat;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception on getting the status",ex);
            }
        }

        async public Task Set(SystemStatus status, DateTime? nextTime)
        {
            try
            {
                DateTime now = DateTime.Now;
                pDbContext.Statuse.AddAsync(new Status {Text=status.ToString(), UpdateTo=now , LottoryEnd = nextTime});
                await pDbContext.SaveChangesAsync();              
            }
            catch (Exception ex)
            {
                throw new Exception("Exception on getting the status", ex);
            }
        }

    }
}
