﻿using System;

namespace EtCoreApi.Entities
{
    public record Expense
    {
        public int ExpenseId { get; init; }
        public DateTimeOffset ExpenseDate { get; set; }
        public decimal ExpenseAmount { get; init; }
        public string ExpenseDetails { get; init; }
    }
}
