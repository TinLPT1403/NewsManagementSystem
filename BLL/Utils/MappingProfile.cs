using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<SystemAccount, AccountDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<AccountDTO, SystemAccount>()
                .ForMember(dest => dest.AccountPasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedArticles, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedArticles, opt => opt.Ignore());

            CreateMap<AccountUpdateAdminDTO, SystemAccount>()
                .ForMember(dest => dest.AccountPasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedArticles, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedArticles, opt => opt.Ignore());

            CreateMap<SystemAccount, AccountUpdateAdminDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // Ignore password for security
        }
    }
}
