using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using server.BL;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Drawing;

namespace server.DAL
{
    public class DonorDal : IDonorDal
    {
        private readonly PDbContext pDbContext;
        private readonly IMapper mapper;
        public DonorDal(PDbContext pDbContext, IMapper mapper)
        {
            this.pDbContext = pDbContext;
            this.mapper = mapper;
        }
        async public Task Add(DonorDTO donorDTO)
        {
            var donor = mapper.Map<Donor>(donorDTO);
            pDbContext.Donors.AddAsync(donor);
            await pDbContext.SaveChangesAsync();
        }

        async public Task<List<DonorDTOResoult>> Get()
        {
            var donors = await pDbContext.Donors.Include(g => g.DonorGifts).ThenInclude(gd => gd.Gift).ToListAsync();
            return mapper.Map<List<DonorDTOResoult>>(donors);
        }

        async public Task<DonorDTOResoult> Get(int id)
        {
            var donors = await pDbContext.Donors.Include(g => g.DonorGifts).ThenInclude(gd => gd.Gift).FirstOrDefaultAsync(g => g.Id == id);
            return mapper.Map<DonorDTOResoult>(donors);
        }

        async public Task<List<DonorDTOResoult>> GetByGift(int giftId)
        {
           var donors = await pDbContext.DonorGift.Where(dg => dg.GiftId == giftId).Select(dg => dg.Donor).Include(g => g.DonorGifts).ThenInclude(gd => gd.Gift).ToListAsync();
            return mapper.Map<List<DonorDTOResoult>>(donors);
        }

        async public Task<List<DonorDTOResoult>> Search(string text)
        {
            var donors = await pDbContext.Donors.Include(g => g.DonorGifts).ThenInclude(gd => gd.Gift).ToListAsync();
            var mappedDonors = mapper.Map<List<DonorDTOResoult>>(donors);
            var resualtDonors = mappedDonors.Where(g => g.Name.Contains(text) || (g.Details!=null && g.Details.Contains(text)) || (g.Email != null && g.Email.Contains(text)) || (g.gifts != null && g.gifts.Any(gg => gg.Title.Contains(text) || (gg.Details != null && gg.Details.Contains(text)))));
            return resualtDonors.ToList();
        }

        async public Task Remove(int id)
        {
            var donor = await pDbContext.Donors.FindAsync(id);
            if (donor != null)
            {
                pDbContext.Donors.Remove(donor);
                await pDbContext.SaveChangesAsync();
            }
        }
        
        async public Task Remove()
        {
            var donor = await pDbContext.Donors.ToListAsync();
            if (donor != null)
            {
                pDbContext.Donors.RemoveRange(donor);
                await pDbContext.SaveChangesAsync();
            }
        }

        async public Task Update(DonorDTO oldDonor, int id)
        {
            var donor = await pDbContext.Donors.FindAsync(id);
            if (donor != null)
            {
                mapper.Map(oldDonor, donor);
                await pDbContext.SaveChangesAsync();
            }
        }

        async public Task<bool> EmailExists(string email)
        {
            return await pDbContext.Donors.AnyAsync(g => g.Email == email);
        }
        async public Task<bool> NameExists(string name)
        {
            return await pDbContext.Donors.AnyAsync(g => g.Name == name);
        }
    }
}
