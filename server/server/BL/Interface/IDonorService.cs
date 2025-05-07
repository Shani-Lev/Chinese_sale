using server.Models.DTO;
using server.Models;

namespace server.BL.Interface
{
    public interface IDonorService
    {
        public Task<List<DonorDTOResoult>> Get();
        public Task<DonorDTOResoult> Get(int id);
        public Task<List<DonorDTOResoult>> GetByGift(int giftId);
        public Task<List<DonorDTOResoult>> Search(string text);
        public Task Add(DonorDTO donor);
        public Task Update(DonorDTO donor, int id);
        public Task Remove(int id);
        public Task Remove();
        public Task<bool> NameExists(string title);
        public Task<bool> EmailExists(string title);
    }
}
