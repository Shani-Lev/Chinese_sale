using server.BL.Interface;
using server.DAL.Interface;
using server.Models.DTO;

namespace server.BL
{
    public class TicketMannagerService : ITicketManagerService
    {
        private readonly ITicketDalMannager ticketDal;
        public TicketMannagerService(ITicketDalMannager ticketDal)
        {
            this.ticketDal = ticketDal;
        }

        async public Task<List<TicketDTOm_Before>> Get()
        {
            try
            {
                return await ticketDal.Get();
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting tickets");
            }
        }

        async public Task<TicketDTOm_Before> Get(int giftId)
        {
            try
            {
                return await ticketDal.Get(giftId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting tickets");
            }
        }

        async public Task<List<UserDTOResualt>> GetUsers(int giftId)
        {
            try
            {
                return await ticketDal.GetUsers(giftId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting users");
            }
        }

        async public Task<List<TicketDTOm_Before>> OrderByPrice()
        {
            try
            {
                return await ticketDal.OrderByPrice();
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting tickets");
            }
        }

        async public Task<List<TicketDTOm_Before>> OrderBySales()
        {
            try
            {
                return await ticketDal.OrderBySales();
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting tickets");
            }
        }
    }
}
