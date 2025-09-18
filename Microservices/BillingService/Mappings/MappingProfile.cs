using AutoMapper;
using BillingService.Models.DTOs;
using BillingService.Models.Entities;

namespace BillingService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Transaction mappings
            CreateMap<Transaction, TransactionDto>();
            CreateMap<CreateTransactionDto, Transaction>();
            CreateMap<UpdateTransactionDto, Transaction>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Expenditure mappings
            CreateMap<Expenditure, ExpenditureDto>();
            CreateMap<CreateExpenditureDto, Expenditure>();
            CreateMap<UpdateExpenditureDto, Expenditure>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Invoice mappings
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<CreateInvoiceDto, Invoice>();
            CreateMap<UpdateInvoiceDto, Invoice>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
