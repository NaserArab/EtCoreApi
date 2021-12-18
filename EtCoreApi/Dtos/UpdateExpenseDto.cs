using System;
using System.ComponentModel.DataAnnotations;

namespace EtCoreApi.Dtos
{
    public record UpdateExpenseDto
    {
        [Required]
        public DateTimeOffset ExpenseDate { get; set; }
        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal ExpenseAmount { get; set; }
        public string ExpenseDetails { get; set; }
    }
}
