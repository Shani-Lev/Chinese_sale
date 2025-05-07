using server.Models;
using server.Models.DTO;

namespace server.DAL.Interface
{
    public interface IAuthDal
    {
        public Task<User> Login(string username, string password); 
        public Task<bool> register(UserDTO user);
        public Task<bool> EmailExsist(string email);
    }
}
