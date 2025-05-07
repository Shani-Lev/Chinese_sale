using server.Models.DTO;

namespace server.BL.Interface
{
    public interface IAuthService
    {
        public Task<string> login(string username, string password);
        public Task<string> register(UserDTO userDTO);
    }
}
