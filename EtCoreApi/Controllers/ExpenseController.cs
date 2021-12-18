using System;
using EtCoreApi.Dtos;
using EtCoreApi.Entities;
using EtCoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EtCoreApi.Controllers
{
    [ApiController]
    [Route("Expense")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpensesRepository iExpensesRepository;

        public ExpenseController(IExpensesRepository _iExpensesRepository)
        {
            this.iExpensesRepository = _iExpensesRepository;
        }

        [HttpPost]
        public ActionResult<CreateExpenseDto> CreateExpense(CreateExpenseDto createExpenseDto)
        {
            Expense expense = createExpenseDto.AsExpense();

            iExpensesRepository.CreateExpense(expense);

            return Ok(CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense.AsDto()));
        }

        // DELETE /expense/{Id}
        [HttpDelete("Id")]
        public ActionResult DeleteExpense(Guid Id)
        {
            var existingExpense = iExpensesRepository.GetExpense(Id);

            if (existingExpense is null)
            {
                return NotFound();
            }

            iExpensesRepository.DeleteExpense(existingExpense.Id);

            return NoContent();
        }

        [HttpGet("{Id}")]
        public ActionResult<ExpenseDto> GetExpense(Guid Id)
        {
            var expense = iExpensesRepository.GetExpense(Id);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense.AsDto());
        }

        [HttpGet]
        public IEnumerable<ExpenseDto> GetExpenses()
        {
            var expenses = iExpensesRepository.GetExpenses().Select(p => p.AsDto());

            return expenses;
        }

        // PUT /expense/{id}
        [HttpPut("{Id}")]
        public ActionResult UpdateExpense(Guid Id, UpdateExpenseDto updateExpenseDto)
        {
            var existingExpense = iExpensesRepository.GetExpense(Id);

            if (existingExpense is null)
            {
                return NotFound();
            }

            Expense updatedExpense = existingExpense with
            {
                ExpenseDetails = updateExpenseDto.ExpenseDetails,
                ExpenseAmount = updateExpenseDto.ExpenseAmount,
                ExpenseDate = updateExpenseDto.ExpenseDate
            };

            iExpensesRepository.UpdateExpense(updatedExpense);

            return NoContent();
        }
    }
}