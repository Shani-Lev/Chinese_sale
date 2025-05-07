using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BL;
using server.BL.Interface;
using server.Models;
using server.Models.DTO;
using System.Drawing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ManagerOnly")]
    // [Authorize(Policy = "RequireSetStatus")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        async public Task<ActionResult<List<Category>>> Get()
        {
            try
            {
                var categories = await categoryService.Get();
                return Ok(categories);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        // POST api/<CategoryController>
        [HttpPost]
        async public Task<ActionResult> Post([FromBody] CategoryDTO category)
        {
            var duplicate = await categoryService.isExsist(category.Name);
            if (duplicate)
                return Conflict($"Gift whith name {category.Name} is exist");
            
            try
            {
                await categoryService.Add(category);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> Put(int id, [FromBody] CategoryDTO category)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");

            try
            {
                await categoryService.Update(id, category);
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

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        async public Task<ActionResult> Delete(int id)
        {
            if (id == null)
                return BadRequest("id is requaierd");
            if (id < 0)
                return BadRequest("not valid id");
            try
            {
                await categoryService.Remove(id);
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
        
        // DELETE api/<CategoryController>/5
        [HttpDelete("")]
        async public Task<ActionResult> DeleteAll()
        {

            try
            {
                await categoryService.Remove();
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
    }
}
