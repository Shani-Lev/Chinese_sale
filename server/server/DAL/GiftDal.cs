using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Security.Cryptography;

namespace server.DAL
{
    public class GiftDal : IGiftDal
    {
        private readonly PDbContext pDbContext;
        private readonly IMapper mapper;
        public GiftDal(PDbContext pDbContext, IMapper mapper)
        {
            this.pDbContext = pDbContext;
            this.mapper = mapper;
        }
        async public Task<List<GiftDTOResualt>> Get()
        {
            try
            {
                var gifts = await pDbContext.Gifts
                    .Include(g => g.DonorGifts)
                        .ThenInclude(dg => dg.Donor)
                    .Include(g => g.GiftCategories)
                        .ThenInclude(gc => gc.Category)
                    .OrderBy(g=>g.Size).Reverse()
                        .ToListAsync();

                return mapper.Map<List<GiftDTOResualt>>(gifts);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the gift.", ex);
            }
        }

        async public Task<GiftDTOResualt> Get(int id)
        {
            try
            {
                var gift = await pDbContext.Gifts
                    .Include(g => g.DonorGifts)
                        .ThenInclude(dg => dg.Donor)
                    .Include(g => g.GiftCategories)
                        .ThenInclude(gc => gc.Category)
                    .FirstOrDefaultAsync(g => g.Id == id);

                return mapper.Map<GiftDTOResualt>(gift);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the gift.", ex);
            }
        }

        async public Task Add(GiftDTO giftDTO)
        {
            try
            {
                var existingDonors = await pDbContext.Donors.Where(d => giftDTO.Donors.Select(dto => dto.Id).Contains(d.Id)).ToListAsync();
                var existingCategories = await pDbContext.Categories.Where(c => giftDTO.Categories.Select(dto => dto.Id).Contains(c.Id)).ToListAsync();

                if (existingDonors.Count != giftDTO.Donors.Count)
                {
                    throw new Exception("Some donors do not exist in the database.");
                }

                if (existingCategories.Count != giftDTO.Categories.Count)
                {
                    throw new Exception("Some categories do not exist in the database.");
                }

                var gift = mapper.Map<Gift>(giftDTO);
                await pDbContext.Gifts.AddAsync(gift);
                await pDbContext.SaveChangesAsync();

                var donorGifts = giftDTO.Donors.Select(d => new DonorGift { DonorId = d.Id, GiftId = gift.Id }).ToList();
                var giftCategories = giftDTO.Categories.Select(c => new GiftCategory { CategoryId = c.Id, GiftId = gift.Id }).ToList();

                gift.DonorGifts = donorGifts;
                gift.GiftCategories = giftCategories;

                pDbContext.Gifts.Update(gift);
                await pDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Database update error occurred while adding the gift.", dbEx);
            }
            catch (ArgumentNullException argEx)
            {
                throw new Exception("A required argument was null while adding the gift.", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the gift.", ex);
            }

        }

        async public Task Remove(int id)
        {
            try
            {
                var gift = await pDbContext.Gifts.FindAsync(id);
                if (gift != null)
                {
                    pDbContext.Gifts.Remove(gift);
                    await pDbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Gift whith id {id} not found");
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Database update error occurred while deleting the gift.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the gift.", ex);
            }

        } 
        
        async public Task Remove()
        {
            try
            {
                var gift = await pDbContext.Gifts.ToListAsync();
                if (gift != null)
                {
                    pDbContext.Gifts.RemoveRange();
                    await pDbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Gift whith id not found");
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Database update error occurred while deleting the gift.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the gift.", ex);
            }

        }

        async public Task Update(GiftDTO OldGift, int id)
        {
            try
            {
                var existingDonors = await pDbContext.Donors.Where(d => OldGift.Donors.Select(dto => dto.Id).Contains(d.Id)).ToListAsync();
                var existingCategories = await pDbContext.Categories.Where(c => OldGift.Categories.Select(dto => dto.Id).Contains(c.Id)).ToListAsync();

                if (existingDonors.Count != OldGift.Donors.Count)
                {
                    throw new Exception("Some donors do not exist in the database.");
                }

                if (existingCategories.Count != OldGift.Categories.Count)
                {
                    throw new Exception("Some categories do not exist in the database.");
                }

                var gift = await pDbContext.Gifts
                .Include(g => g.DonorGifts)
                .Include(g => g.GiftCategories)
                .FirstOrDefaultAsync(g => g.Id == id);

                if (gift != null)
                {
                    if (gift.DonorGifts != null)
                    {
                        pDbContext.DonorGift.RemoveRange(gift.DonorGifts);
                    }

                    if (gift.GiftCategories != null)
                    {
                        pDbContext.giftCategories.RemoveRange(gift.GiftCategories);
                    }

                    await pDbContext.SaveChangesAsync();

                    mapper.Map(OldGift, gift);

                    gift.DonorGifts = OldGift.Donors.Select(d => new DonorGift { DonorId = d.Id, GiftId = gift.Id }).ToList();
                    gift.GiftCategories = OldGift.Categories.Select(c => new GiftCategory { CategoryId = c.Id, GiftId = gift.Id }).ToList();


                    await pDbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Database update error occurred while adding the gift.", dbEx);
            }
            catch (ArgumentNullException argEx)
            {
                throw new Exception("A required argument was null while adding the gift.", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the gift.", ex);
            }

        }
        async public Task<List<Donor>> GetDonors(int id)
        {

            return await pDbContext.DonorGift.Where(dg => dg.GiftId == id).Select(dg => dg.Donor).ToListAsync();
        }

        async public Task<bool> TitleExists(string title)
        {
            return await pDbContext.Gifts.AnyAsync(g => g.Title == title);
        }

        async public Task<List<GiftDTOResualt>> Search(string name = "", string donor = "", int minSales = 0, int price = 10)
        {
            var giftsWithBuyerCount = await pDbContext.Tickets
                .GroupBy(t => t.GiftId)
                .Select(g => new { GiftId = g.Key, BuyerCount = g.Count() })
                .ToListAsync();

            var giftIds = giftsWithBuyerCount
                .Where(g => g.BuyerCount >= minSales)
                .Select(g => g.GiftId)
                .ToList();

            var searchedGifts = await pDbContext.Gifts
                .Where(g => ((string.IsNullOrEmpty(name) || g.Title.Contains(name) || g.Details.Contains(name))) &&
                             ((string.IsNullOrEmpty(donor) || g.DonorGifts.Any(gd => gd.Donor.Name.Contains(donor)))) &&
                             giftIds.Contains(g.Id) &&
                             g.Price == price)
                .Include(g => g.DonorGifts)
                .ThenInclude(gd => gd.Donor)
                .ToListAsync();

            return mapper.Map<List<GiftDTOResualt>>(searchedGifts);
        }

        async public Task<List<GiftDTOResualt>> SortByPrice()
        {
            var gifts = await this.Get();
            return gifts.OrderBy(g => g.Size).ToList();
        }

        async public Task<List<GiftDTOResualt>> SortByName()
        {
            var gifts = await this.Get();
            return gifts.OrderBy(g => g.Title).ToList();
        }

        //async public Task<List<Gift>> SearchByName(string name)
        //{
        //    return await pDbContext.Gifts.Where(g => g.Title.Contains(name) || g.Details.Contains(name)).ToListAsync();
        //}

        //async public Task<List<Gift>> SearchByDonor(string name)
        //{
        //    return await pDbContext.Gifts.Include(g => g.DonorGifts).ThenInclude(gd => gd.Donor).Where(g => g.DonorGifts.Any(gd => gd.Donor.Name.Contains(name))).ToListAsync();
        //}

        //async public Task<List<Gift>> SearchBySales(int min)
        //{
        //    var giftsWithBuyerCount = await pDbContext.Tickets.GroupBy(t => t.GiftId).Select(g => new{ GiftId = g.Key,  BuyerCount = g.Count() }).ToListAsync();

        //    var giftIds = giftsWithBuyerCount.Where(g => g.BuyerCount > min).Select(g => g.GiftId).ToList();

        //    return await pDbContext.Gifts.Where(g => giftIds.Contains(g.Id)).ToListAsync();
        //}

        //async public Task<List<Gift>> SearchByPrice(int price)
        //{
        //    return await pDbContext.Gifts.Where(g => g.Price == price).ToListAsync();
        //}


    }
}
