using System;

namespace EtCoreApi.Dtos
{
    public record CreateExpenseDto
    {
        public DateTimeOffset ExpenseDate { get; set; }
        public decimal ExpenseAmount { get; init; }
        public string ExpenseDetails { get; init; }
    }
}
