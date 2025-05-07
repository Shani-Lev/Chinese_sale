using server.BL.Interface;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.BL
{
    public class GiftService : IGiftService
    {
        private readonly IGiftDal giftDal;
        
        public GiftService(IGiftDal giftDal)
        {
            this.giftDal = giftDal;
        }

        async public Task Add(GiftDTO gift)
        {
            await giftDal.Add(gift);
        }

        async public Task<List<GiftDTOResualt>> Get()
        {
            return await giftDal.Get();
        }

        async public Task<GiftDTOResualt> Get(int id)
        {
            return await giftDal.Get(id);
        }

        async public Task Remove(int id)
        {
            await giftDal.Remove(id);
        }
        
        async public Task Remove()
        {
            await giftDal.Remove();
        }

        async public Task Update(GiftDTO gift, int id)
        {
            await giftDal.Update(gift, id);
        }

        async public Task<List<GiftDTOResualt>> Search(string name = "", string donor = "", int minSales = 0, int price = 10)
        {
            return await giftDal.Search(name, donor, minSales, price); 
        }

        async public Task<List<GiftDTOResualt>> OrderByPrice()
        {
            return await giftDal.SortByPrice();
        }
        
        async public Task<List<GiftDTOResualt>> OrderByName()
        {
            return await giftDal.SortByName();
        }

        async public Task<bool> CheckIfTitleExists(string title)
        {
            return await giftDal.TitleExists(title);
        }
    }
}
