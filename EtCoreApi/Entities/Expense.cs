using System;
using System.ComponentModel.DataAnnotations;

namespace EtCoreApi.Entities
{
    public record Expense
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTimeOffset ExpenseDate { get; set; }

        [Required]
        public decimal ExpenseAmount { get; set; }

        public string ExpenseDetails { get; set; }
    }
}
