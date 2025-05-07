using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using server.BL.Interface;
using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  
    public class TicketUserController : ControllerBase
    {
        private readonly ITicketUserService ticketUserService;
        private readonly ILogger<TicketUserController> logger;

        public TicketUserController(ITicketUserService ticketUserService, ILogger<TicketUserController> logger)
        {
            this.ticketUserService = ticketUserService;
            this.logger = logger;
        }

        // POST api/<TicketUserController>
        [HttpPost]
        [Authorize(Policy = "RequireOnStatus")]
        async public Task<ActionResult> Add(int giftId, int Amount, bool toBasket)
        {
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                logger.LogError("Authorization header is missing.");
                return Forbid("not valid header: autorization", HttpContext.Request.Headers.ToString());
            }
            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                logger.LogError("Invalid Authorization header format.");
                return Forbid("Invalid Authorization header format: " + authHeader);
            }
            string token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                logger.LogError("Token is empty after extraction.");
                return Forbid("Token is empty after extraction.");
            }

            int userId = GetUserIdFromToken(token);
             
            try
            {
                await ticketUserService.Add(userId, giftId, Amount, toBasket);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<TicketUserController>/{id}
        [HttpPut("{giftId}")]
        [Authorize(Policy = "RequireOnStatus")]
        async public Task<ActionResult> Buy(int giftId)
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader[7..];
            int userId = GetUserIdFromToken(token);
            try
            {
                await ticketUserService.Buy(userId, giftId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<TicketUserController>
        [HttpPut("")]
        [Authorize(Policy = "RequireOnStatus")]
        async public Task<ActionResult> Buy()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader[7..];
            int userId = GetUserIdFromToken(token);
            try
            {
                await ticketUserService.Buy(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<TicketUserController>/user/{userId}
        [HttpGet("basket/")]
        [Authorize(Policy = "RequireOnStatus")]
        async public Task<ActionResult<List<TicketDTOResualt>>> GetInBasket()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader[7..];
            int userId = GetUserIdFromToken(token);

            try
            {
                var tickets = await ticketUserService.GetInBasket(userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        async public Task<ActionResult<List<TicketDTOResualt>>> GetNotInBasket()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader[7..];
            int userId = GetUserIdFromToken(token);

            try
            {
                var tickets = await ticketUserService.GetNotInBasket(userId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<TicketUserController>/remove/{id}
        [HttpDelete()]
        [Authorize(Policy = "RequireOnStatus")]
        async public Task<ActionResult> Delete(int giftId, int amount)
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            string token = authHeader[7..];
            int userId = GetUserIdFromToken(token);
            try
            {
                await ticketUserService.Remove(userId, giftId, amount);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private void TesrAuthorizatoin()
        {
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                logger.LogError("Authorization header is missing.");
                //return 0;
            }
        }
        private int GetUserIdFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(jwtToken))
            {
                try
                {
                    var jwtTokenObject = handler.ReadJwtToken(jwtToken);
                    var idClaim = jwtTokenObject.Claims.FirstOrDefault(claim => claim.Type == "nameid");

                    if (idClaim != null)
                    {
                        logger.LogDebug($"Found claim: {idClaim.Value}");

                        if (int.TryParse(idClaim.Value, out int userId))
                        {
                            logger.LogDebug($"User ID successfully parsed: {userId}");
                            return userId;
                        }
                        else
                        {
                            logger.LogError($"User ID claim value is not a valid integer: {idClaim.Value}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error parsing token: {ex.Message}");
                }
            }
            else
            {
                logger.LogError("Cannot read token, invalid format.");
            }
            logger.LogError("Error on reading token");
            return 0; 
        }
    }
}
