using EtCoreApi.Dtos;
using EtCoreApi.Entities;

namespace EtCoreApi
{
    public static class Extentions
    {
        public static ExpenseDto AsDto(this Expense expense)
        {
            return new ExpenseDto
            {
                Id = expense.Id,
                ExpenseDetails = expense.ExpenseDetails,
                ExpenseAmount = expense.ExpenseAmount,
                ExpenseDate = expense.ExpenseDate
            };
        }

        public static Expense AsExpense(this ExpenseDto expenseDto)
        {
            return new Expense
            {
                Id = expenseDto.Id,
                ExpenseDetails = expenseDto.ExpenseDetails,
                ExpenseAmount = expenseDto.ExpenseAmount,
                ExpenseDate = expenseDto.ExpenseDate
            };
        }

        public static Expense AsExpense(this CreateExpenseDto createExpenseDto)
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
