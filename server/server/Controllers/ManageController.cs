using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BL;
using server.BL.Interface;
using server.Models;
using server.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ManageController : ControllerBase
    {
        private readonly IManageService manageService;
        private readonly ILogger<ManageController> logger;

        public ManageController(IManageService manageService, ILogger<ManageController> logger)
        {
            this.manageService = manageService;
            this.logger = logger;
        }

        // GET: api/<ManageController>/winners/{giftId}
        [HttpGet("winners")]
        [Authorize(Policy = "RequireOffStatus")]
        [Authorize(Policy = "ManagerOnly")]
        async public Task<ActionResult<List<TicketDTOm_After>>> GetWinners()
        {
            try
            {
                var winners = await manageService.GetWinners();
                return Ok(winners);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ManageController>/lottery/{giftId}
        [HttpPut("lottery/{giftId}")]
        [Authorize(Policy = "RequireOffStatus")]
        [Authorize(Policy = "ManagerOnly")]
        async public Task<ActionResult<UserDTOResualt>> SetLottery(int giftId)
        {
            try
            {
                var result = await manageService.SetLottery(giftId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // POST api/<ManageController>/lottery/
        [HttpPut("lottery")]
        [Authorize(Policy = "RequireOffStatus")]
        [Authorize(Policy = "ManagerOnly")]
        async public Task<ActionResult> SetLottery()
        {
            try
            {
                await manageService.SetLottery();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<ManageController>/revenue/{giftId}
        [HttpGet("revenue")]
        [Authorize(Policy = "RequireOffStatus")]
        [Authorize(Policy = "ManagerOnly")]
        async public Task<ActionResult<List<RevenueReport>>> GetRevenue()
        {
            try
            {
                var revenue = await manageService.GetRevenue();
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<ManageController>/status/{giftId}
        [HttpGet("status")]
        async public Task<ActionResult<Status>> GetStatus()
        {
            try
            {
                var status = await manageService.GetStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ManageController>/status
        [HttpPost("status")]
        [Authorize(Policy = "ManagerOnly")]
        async public Task<ActionResult> SetStatus([FromBody] DateTime nextTime)
        {
            try
            {
                await manageService.SetStatus(nextTime, 0);
                
                return Ok();
            }
            catch (ArgumentException ex){
                return BadRequest("the next time is not valid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ManageController>/status
        [HttpPut("status")]
        [Authorize(Policy = "ManagerOnly")]
        async public Task<ActionResult> UpdateStatuse([FromBody] DateTime nextTime)
        {
            try
            {
                logger.LogInformation("nextTime: " + nextTime);
                //nextTime = ParseHebrewDate(nextTime);
                await manageService.SetStatus(nextTime, 1);
 
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest("the next time is not valid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

