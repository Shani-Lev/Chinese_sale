using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BL.Interface;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ManagerOnly")]

    public class TicketManagerController : ControllerBase
    {
        private readonly ITicketManagerService ticketManagerService;

        public TicketManagerController(ITicketManagerService ticketManagerService)
        {
            this.ticketManagerService = ticketManagerService;
        }

        // GET: api/<TicketManagerController>
        [HttpGet]
        [Authorize(Policy = "RequireOnStatus")]
        async public Task<ActionResult<List<TicketDTOm_Before>>> Get()
        {
            try
            {
                var tickets = await ticketManagerService.Get();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<TicketManagerController>/gift/{giftId}
        [HttpGet("{giftId}")]
        [Authorize(Policy = "RequireOnStatus")]
        async public Task<ActionResult<TicketDTOm_Before>> Get(int giftId)
        {
            try
            {
                var ticket = await ticketManagerService.Get(giftId);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<TicketManagerController>/users/{giftId}
        [HttpGet("users/{giftId}")]
        async public Task<ActionResult<List<UserDTOResualt>>> GetUsers(int giftId)
        {
            try
            {
                var users = await ticketManagerService.GetUsers(giftId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<TicketManagerController>/orderByPrice
        [HttpGet("orderByPrice")]
        async public Task<ActionResult<List<TicketDTOm_Before>>> OrderByPrice()
        {
            try
            {
                var tickets = await ticketManagerService.OrderByPrice();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<TicketManagerController>/orderBySales
        [HttpGet("orderBySales")]
        async public Task<ActionResult<List<TicketDTOm_Before>>> OrderBySales()
        {
            try
            {
                var tickets = await ticketManagerService.OrderBySales();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

