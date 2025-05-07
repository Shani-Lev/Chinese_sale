using server.BL.Interface;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.BL
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDal categoryDal;
        
        public CategoryService(ICategoryDal categoryDal)
        {
            this.categoryDal = categoryDal;
        }
        
        async public Task Add(CategoryDTO categoryDTO)
        {
            await categoryDal.Add(categoryDTO);
        }

        async public Task<List<Category>> Get()
        {
            return await categoryDal.Get();
        }

        async public Task Remove(int id)
        {
            await categoryDal.Remove(id);
        }
        async public Task Remove()
        {
            await categoryDal.Remove();
        }

        async public Task Update(int id, CategoryDTO categoryDTO)
        {
            await categoryDal.Update(id, categoryDTO);
        }

        async public Task<bool> isExsist(string name)
        {
            return await categoryDal.idExsist(name);
        }
    }
}
