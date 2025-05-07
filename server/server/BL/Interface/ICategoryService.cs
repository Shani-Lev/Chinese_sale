using server.Models.DTO;
using server.Models;

namespace server.BL.Interface
{
    public interface ICategoryService
    {
        public Task<List<Category>> Get();
        public Task Add(CategoryDTO categoryDTO);
        public Task Update(int id, CategoryDTO categoryDTO);
        public Task Remove(int id);
        public Task Remove();

        public Task<bool> isExsist(string name);
    }
}
