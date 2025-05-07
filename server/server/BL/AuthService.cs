using Microsoft.IdentityModel.Tokens;
using server.BL.Interface;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace server.BL
{
    public class AuthService : IAuthService
    {
        private readonly IAuthDal authDal;
        public AuthService(IAuthDal authDal)
        {
            this.authDal = authDal; 
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ph/MB07NAZWbwLwYb/kCzDWJHIHMJra/KZ1+nrTRym4=");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        async public Task<string> login(string username, string password)
        {
            try
            {
                var user = await authDal.Login(username, password);
                    if (user != null)
                    {
                        var token = GenerateToken(user);
                        return token.ToString();
                    }
                    else 
                    {
                    return null;//throw new InvalidOperationException("User not found");
                    }
            }
            catch (Exception ex)
            {
                throw new Exception("An error, user not found", ex);
            }
            
        }
        
        async public Task<string> register(UserDTO userDTO)
        {
            try
            {
                var conf = await authDal.EmailExsist(userDTO.Email);
                if (!conf)
                {
                    var validationMessage = Validation(userDTO);
                    if (validationMessage == "")
                    {
                        await authDal.register(userDTO);
                    }
                    else
                    {
                        throw new Exception(validationMessage);
                    }
                }
                else
                {
                    throw new Exception("This email is exsist");
                }
                return "User added";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message ,ex);
            }

        }

        public string Validation(UserDTO user)
        {
            StringBuilder errors = new StringBuilder();
            if (!Regex.IsMatch(user.Password, @"^(?=.*[a-zA-Z])(?=.*\d).+$"))
            {
                errors.Append("Password mast contains characters and numbers. ");
            }
            if (user.Phone.Length < 9 || user.Phone.Length > 10)
            {
                errors.Append("Not valid phone. ");
            }
            if (user.Email.IndexOf("@") == -1 || user.Email.LastIndexOf(".") < user.Email.IndexOf("@")+1 )
            {
                errors.Append("Not valid email. ");
            }
            if (user.Name.Length < 3)
            {
                errors.Append("Name is too short. ");
            }
            if (user.Password.Length < 4)
            {
                errors.Append("Password is too short. ");
            }

            return errors.ToString();
        }
    }
}
