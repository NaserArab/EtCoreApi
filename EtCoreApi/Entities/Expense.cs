using System;

namespace EtCoreApi.Entities
{
    public record Expense
    {
        public Guid Id { get; set; }
        public DateTimeOffset ExpenseDate { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string ExpenseDetails { get; set; }
    }
}
