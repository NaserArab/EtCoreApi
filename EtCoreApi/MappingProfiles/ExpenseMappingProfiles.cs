using AutoMapper;
using EtCoreApi.Dtos;
using EtCoreApi.Entities;

namespace EtCore.Api.MappingProfiles
{
    public class ExpenseMappingProfiles : Profile
    {
        public ExpenseMappingProfiles()
        {
            //Source ==> Target
            CreateMap<Expense, ExpenseReadDto>();
            CreateMap<ExpenseCreateDto, Expense>();
        }
    }
}