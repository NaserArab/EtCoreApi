using EtCoreApi.Dtos;
using EtCoreApi.Entities;

namespace EtCoreApi
{
    public static class Extentions
    {
        public static ExpenseReadDto AsDto(this Expense expense)
        {
            return new ExpenseReadDto
            {
                Id = expense.Id,
                ExpenseDetails = expense.ExpenseDetails,
                ExpenseAmount = expense.ExpenseAmount,
                ExpenseDate = expense.ExpenseDate
            };
        }

        public static Expense AsExpense(this ExpenseReadDto expenseDto)
        {
            return new Expense
            {
                Id = expenseDto.Id,
                ExpenseDetails = expenseDto.ExpenseDetails,
                ExpenseAmount = expenseDto.ExpenseAmount,
                ExpenseDate = expenseDto.ExpenseDate
            };
        }

        public static Expense AsExpense(this ExpenseCreateDto createExpenseDto)
        {
            return new Expense
            {
                ExpenseDetails = createExpenseDto.ExpenseDetails,
                ExpenseAmount = createExpenseDto.ExpenseAmount,
                ExpenseDate = createExpenseDto.ExpenseDate
            };
        }
    }
}
