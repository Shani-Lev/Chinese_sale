using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server.DAL
{
    public class AuthDal : IAuthDal
    {
        public readonly PDbContext pDbContext;
        public readonly IMapper mapper;
        public AuthDal(PDbContext pDbContext, IMapper mapper)
        {
            this.pDbContext = pDbContext;
            this.mapper = mapper;
        }

        async public Task<User> Login(string username, string password)
        {
            try
            {
                var user = await pDbContext.Users.FirstOrDefaultAsync(u => u.Email == username && u.Password == password);
                //if (user == null)
                //{
                //    throw new Exception("User not found. Please check your username.");
                //}
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error on login", ex);
            }

        }

        async public Task<bool> register(UserDTO userDTO)
        {
            try
            {
                var user = mapper.Map<User>(userDTO);
                pDbContext.Users.AddAsync(user);
                await pDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                //return false;
                throw new Exception("An error on adding user", ex);
            }
        }

        async public Task<bool> EmailExsist(string email)
        {
            return await pDbContext.Users.AnyAsync(u => u.Email == email);
        }
    }
}
