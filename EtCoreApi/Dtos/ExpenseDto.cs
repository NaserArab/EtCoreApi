using System;

namespace EtCoreApi.Dtos
{
    public record ExpenseDto
    {
        public int ExpenseId { get; init; }
        public DateTimeOffset ExpenseDate { get; set; }
        public decimal ExpenseAmount { get; init; }
        public string ExpenseDetails { get; init; }
    }
}
