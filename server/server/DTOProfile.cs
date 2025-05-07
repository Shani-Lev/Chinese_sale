using AutoMapper;
using server.Models;
using server.Models.DTO;
namespace server
{
    public class DTOProfile : Profile
    {
        public DTOProfile()
        {
            {
                CreateMap<GiftDTO, Gift>()
                    .ForMember(dest => dest.DonorGifts, opt => opt.MapFrom(src => src.Donors.Select(d => new DonorGift { DonorId = d.Id }).ToList()))
                    .ForMember(dest => dest.GiftCategories, opt => opt.MapFrom(src => src.Categories.Select(c => new GiftCategory { CategoryId = c.Id }).ToList()));

                CreateMap<Gift, GiftDTO>()
                     .ForMember(dest => dest.Donors, opt => opt.MapFrom(src => src.DonorGifts.Select(dg => new DonorDTOResoult
                     {
                         Id = dg.Donor.Id,
                         Name = dg.Donor.Name,
                         Details = dg.Donor.Details,
                         Phone = dg.Donor.Phone,
                         Email = dg.Donor.Email,
                         Logo = dg.Donor.Logo,
                         ShowMe = dg.Donor.ShowMe
                     })))
                     .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.GiftCategories.Select(gc => new CategoryDTOResoult
                     {
                         Id = gc.Category.Id,
                         Name = gc.Category.Name
                     })));
                CreateMap<Gift, GiftDTOResualt>()
                .ForMember(dest => dest.Donors, opt => opt.MapFrom(src => src.DonorGifts.Select(dg => dg.Donor)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.GiftCategories.Select(gc => gc.Category)));
                CreateMap<Gift, GiftDTOTheen>();
                
                CreateMap<TicketDTO, Ticket>();
                CreateMap<Ticket, TicketDTOResualt>();
                CreateMap<Ticket, TicketDTOm_After>();

                CreateMap<CategoryDTOResoult, Category>();
                CreateMap<Category, CategoryDTOResoult>();
                CreateMap<CategoryDTO, Category>();

                CreateMap<DonorDTOResoult, Donor>();
                CreateMap<Donor, DonorDTOResoult>()
                    .ForMember(dest => dest.gifts, opt => opt.MapFrom(src =>
                        src.DonorGifts.Select(gc => new GiftDTOTheen
                        {
                            Id = gc.Gift.Id,
                            Title = gc.Gift.Title,
                            Details = gc.Gift.Details,
                            Size = gc.Gift.Size,
                            Price = gc.Gift.Price,
                            Image = gc.Gift.Image,
                        }).ToList()));

                CreateMap<DonorDTO, Donor>();

                CreateMap<User, UserDTOResualt>();
                CreateMap<UserDTO, User>()
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"));
            }
        }
    }
}
