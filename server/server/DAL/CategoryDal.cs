using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;

namespace server.DAL
{
    public class CategoryDal : ICategoryDal
    {
        private readonly PDbContext pDbContext;
        private readonly IMapper mapper;
        private readonly ILogger<CategoryDal> logger;
        public CategoryDal(PDbContext pDbContext, IMapper mapper, ILogger<CategoryDal> logger)
        {
            this.pDbContext = pDbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        async public Task Add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                pDbContext.Categories.Add(category);
                await this.pDbContext.SaveChangesAsync();
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message);
            }
        }

        async public Task<List<Category>> Get()
        {
            try
            {
                return await pDbContext.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        async public Task Remove(int id)
        {
            var category = await pDbContext.Categories.FindAsync(id);
            if (category != null)
            {
                pDbContext.Categories.Remove(category);
                await pDbContext.SaveChangesAsync();
            }
        }
        
        async public Task Remove()
        {
            var category = await pDbContext.Categories.ToListAsync();
            if (category != null)
            {
                pDbContext.Categories.RemoveRange(category);
                await pDbContext.SaveChangesAsync();
            }
        }

        async public Task Update(int id, CategoryDTO categoryDTO)
        {
            var category = await pDbContext.Categories.FindAsync(id);
            if (category != null)
            {
                mapper.Map(categoryDTO, category);
                await pDbContext.SaveChangesAsync();
            }
        }

        async public Task<bool> idExsist(string name)
        {
            return await pDbContext.Categories.AnyAsync(g => g.Name == name);
        }
    }
}
