using server.BL.Interface;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.BL
{
    public class DonorService : IDonorService
    {
        private readonly IDonorDal donorDal;
        public DonorService(IDonorDal donorDal) 
        {
            this.donorDal = donorDal;
        }
        async public Task Add(DonorDTO donor)
        {
            await donorDal.Add(donor);
        }

        async public Task<bool> EmailExists(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            return await donorDal.EmailExists(email);
        }

        async public Task<List<DonorDTOResoult>> Get()
        {
            return await donorDal.Get();
        }

        async public Task<DonorDTOResoult> Get(int id)
        {
            return await donorDal.Get(id);
        }

        async public Task<List<DonorDTOResoult>> GetByGift(int giftId)
        {
            return await donorDal.GetByGift(giftId);
        }

        async public Task<bool> NameExists(string title)
        {
            return await donorDal.NameExists(title);
        }

        async public Task Remove(int id)
        {
            await donorDal.Remove(id);
        }
        
        async public Task Remove()
        {
            await donorDal.Remove();
        }

        async public Task<List<DonorDTOResoult>> Search(string text)
        {
            return await donorDal.Search(text);
        }

        async public Task Update(DonorDTO donor, int id)
        {
            await donorDal.Update(donor, id);
        }
    }
}
