using server.Models.DTO;
using server.Models;

namespace server.BL.Interface
{
    public interface IGiftService
    {
        public Task<List<GiftDTOResualt>> Get();
        public Task<GiftDTOResualt> Get(int id);
        public Task Add(GiftDTO gift);
        public Task Update(GiftDTO gift, int id);
        public Task Remove(int id);
        public Task Remove();
        public Task<List<GiftDTOResualt>> Search(string name = "", string donor = "", int minSales = 0, int price = 10);
        public Task<List<GiftDTOResualt>> OrderByPrice();
        public Task<List<GiftDTOResualt>> OrderByName();
        public Task<bool> CheckIfTitleExists(string title);
    }
}
