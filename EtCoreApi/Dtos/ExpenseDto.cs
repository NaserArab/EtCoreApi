using System;
using System.ComponentModel.DataAnnotations;

namespace EtCoreApi.Dtos
{
    public record ExpenseDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset ExpenseDate { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string ExpenseDetails { get; set; }
    }
}
