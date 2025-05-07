using server.Models;
using server.Models.DTO;

namespace server.DAL.Interface
{
    public interface ICategoryDal
    {
        public Task<List<Category>> Get();
        public Task Add(CategoryDTO categoryDTO);
        public Task Update(int id, CategoryDTO categoryDTO);
        public Task Remove(int id);
        public Task Remove();
        public Task<bool> idExsist(string name);
    }
}
