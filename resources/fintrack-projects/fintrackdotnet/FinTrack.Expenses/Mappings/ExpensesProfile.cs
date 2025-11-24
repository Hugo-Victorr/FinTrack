using AutoMapper;
using FinTrack.Expenses.DTOs;
using FinTrack.Model.Entities;

namespace FinTrack.Expenses.Mappings;

public class ExpensesProfile : Profile
{
    public ExpensesProfile()
    {
        // Expense mappings
        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.ExpenseCategory, opt => opt.MapFrom(src => src.ExpenseCategory));

        CreateMap<ExpenseCreateDto, Expense>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.ExpenseCategory, opt => opt.Ignore())
            .ForMember(dest => dest.Wallet, opt => opt.Ignore());

        CreateMap<ExpenseUpdateDto, Expense>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.ExpenseCategory, opt => opt.Ignore())
            .ForMember(dest => dest.Wallet, opt => opt.Ignore());

        // ExpenseCategory mappings
        CreateMap<ExpenseCategory, ExpenseCategoryDto>();
        CreateMap<ExpenseCategoryCreateDto, ExpenseCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<ExpenseCategoryUpdateDto, ExpenseCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // Wallet mappings
        CreateMap<Wallet, WalletDto>();
        CreateMap<WalletCreateDto, Wallet>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<WalletUpdateDto, Wallet>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}

