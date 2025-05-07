using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using server.BL;
using server.BL.Interface;
using server.Migrations;
using server.Models;
using server.Models.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ManagerOnly")]
    //[Authorize(Policy = "RequireSetStatus")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService donorService;
        public DonorController(IDonorService donorService) 
        { 
            this.donorService = donorService;
        }
        // GET: api/<DonorController>
        [HttpGet]
        async public Task<ActionResult<DonorDTOResoult>> Get()
        {
            try
            {
                var donors = await donorService.Get();
                return Ok(donors);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/<DonorController>/5
        [HttpGet("{id}")]
        async public Task<ActionResult<DonorDTOResoult>> Get(int id)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");
            try
            {
                var donor = await donorService.Get(id);
                return Ok(donor);
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

        // POST api/<DonorController>
        [HttpPost]
        async public Task<ActionResult> Post([FromBody] DonorDTO donorDTO)
        {
            var duplicateN = await donorService.NameExists(donorDTO.Name);
            if (duplicateN)
                return Conflict($"Gift whith name {donorDTO.Name} is exist");
            var duplicateE = await donorService.EmailExists(donorDTO.Email);
            if (duplicateE)
                return Conflict($"Gift whith email {donorDTO.Email} is exist");
            try
            {
                await donorService.Add(donorDTO);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/<DonorController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> Put(int id, [FromBody] DonorDTO donor)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");

            try
            {
                await donorService.Update(donor, id);
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

        // DELETE api/<DonorController>/5
        [HttpDelete("{id}")]
        async public Task<ActionResult> Delete(int id)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");
            try
            {
                await donorService.Remove(id);
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
        // DELETE api/<DonorController>/5
        [HttpDelete("")]
        async public Task<ActionResult> DeleteAll()
        {
            try
            {
                await donorService.Remove();
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

        // GET api/<DonorController>/5
        [HttpGet("search/{text}")]
        async public Task<ActionResult<List<DonorDTOResoult>>> Search(string text)
        {
            try
            {
                var donor = await donorService.Search(text);
                return Ok(donor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
