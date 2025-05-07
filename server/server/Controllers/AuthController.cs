using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using server.BL.Interface;
using server.Models;
using server.Models.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private  readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }
        // POST api/<AuthController>
        [HttpPost]
        [Route("login")]
        async public Task<ActionResult<string>> Login(LoginDTO login)
        {
            try
            {
                var token = await authService.login(login.username, login.password);
                if (token == null)
                {
                    logger.LogError(@$"[user ({login.username}) not entering token]");
                    return BadRequest("Token did not found. please login.");
                }
                logger.LogInformation(@$"[user ({login.username}) login successfully]");
                return Ok(new { token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError(@$"[user ({login.username}) is forbid, {ex.Message}]");
                return Forbid("User not found. Please check your username." + ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(@$"[user ({login.username}) have an error in login, {ex.Message}]");
                return Forbid(ex.Message);
            } 
        }
        [HttpPost]
        [Route("register")]
        async public Task<ActionResult<string>> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                await authService.register(userDTO);
                logger.LogInformation(@$"[user ({userDTO.Name}) is register successfully]");
                return Created();
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(@$"[user ({userDTO.Name}) send not valid parameters, {ex.Message}]");
                return BadRequest("פרמטר חסר: " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(@$"[user ({userDTO.Name}) have an error in register, {ex.Message}]");
                return BadRequest(ex.Message);
            }
        }


    }
}
