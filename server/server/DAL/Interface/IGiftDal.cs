using server.Models;
using server.Models.DTO;

namespace server.DAL.Interface
{
    public interface IGiftDal
    {
        public Task<List<GiftDTOResualt>> Get();
        public Task<GiftDTOResualt> Get(int id);
        public Task Add(GiftDTO gift);
        public Task Update(GiftDTO gift, int id);
        public Task Remove(int id);
        public Task Remove();
        public Task<List<Donor>> GetDonors(int id);
        public Task<List<GiftDTOResualt>> Search(string name = "", string donor = "", int minSales = 0, int price = 10);
        public Task<bool> TitleExists(string title);
        public Task<List<GiftDTOResualt>> SortByPrice();
        public Task<List<GiftDTOResualt>> SortByName();


        //public Task<List<Gift>> SearchByName(string name);
        //public Task<List<Gift>> SearchByDonor(string name);
        //public Task<List<Gift>> SearchBySales(int min);
        //public Task<List<Gift>> SearchByPrice(int price);


    }
}
