using AutoMapper;
using OpenFinance.Model.Dtos;
using OpenFinance.Model.Entities;

namespace OpenFinance.Model.Mappings
{
    /// <summary>
    /// Perfil de mapeamento AutoMapper para as entidades do modelo.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Account Mappings
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.FinancialOrganizationId, 
                    opt => opt.MapFrom(src => src.Financial != null ? src.Financial.Id : Guid.Empty))
                .ForMember(dest => dest.FinancialOrganizationName,
                    opt => opt.MapFrom(src => src.Financial != null ? src.Financial.BrandName : string.Empty));

            CreateMap<AccountDto, Account>();

            // Costumer Mappings
            CreateMap<Costumer, CostumerDto>();
            CreateMap<CostumerDto, Costumer>();

            // FinancialOrganization Mappings
            CreateMap<FinancialOrganization, FinancialOrganizationDto>();
            CreateMap<FinancialOrganizationDto, FinancialOrganization>();
        }
    }
}
