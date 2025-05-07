using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using server.BL.Interface;
using server.Models;
using server.Models.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {

        private readonly IGiftService giftService;
        public GiftController(IGiftService giftService)
        {
            this.giftService = giftService;
        }
        // GET: api/<GiftController>
        [HttpGet]
        async public Task<ActionResult<List<GiftDTOResualt>>> Get()
        {
            try
            {
                var gifts = await giftService.Get();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<GiftController>/5
        [HttpGet("{id}")]
        async public Task<ActionResult<GiftDTOResualt>> Get(int id)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");
            try
            {
                var gift = await giftService.Get(id);
                return Ok(gift);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST api/<GiftController>
        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        // [Authorize(Policy = "RequireSetStatus")]
        async public Task<ActionResult> Post([FromBody] GiftDTO gift)
        {
            var duplicate = await giftService.CheckIfTitleExists(gift.Title);
            if (duplicate)
                return Conflict($"Gift whith title {gift.Title} is exist");
            try
            {
                await giftService.Add(gift);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/<GiftController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        [Authorize(Policy = "RequireSetStatus")]
        async public Task<ActionResult> Put(int id, [FromBody] GiftDTO gift)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");

            try
            {
                await giftService.Update(gift, id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<GiftController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        [Authorize(Policy = "RequireSetStatus")]
        async public Task<ActionResult> Delete(int id)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");
            try
            {
                await giftService.Remove(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 
        
        // DELETE api/<GiftController>/5
        [HttpDelete("")]
        [Authorize(Policy = "ManagerOnly")]
        [Authorize(Policy = "RequireSetStatus")]
        async public Task<ActionResult> Delete()
        {
            try
            {
                await giftService.Remove();
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<GiftController>?name=value&donor=value&minSales=value&price=value
        [HttpGet("search")]
        async public Task<ActionResult<List<GiftDTOResualt>>> Get(string name = "", string donor = "", int minSales = 0, int price = 10)
        {
            try
            {
                var gifts = await giftService.Search(name, donor, minSales, price);
                return Ok(gifts);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // GET: api/<GiftController>
        [HttpGet("sort")]
        async public Task<ActionResult<List<GiftDTOResualt>>> Sort()
        {
            try
            {
                    var gifts = await giftService.OrderByPrice();
                    return Ok(gifts);          
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
