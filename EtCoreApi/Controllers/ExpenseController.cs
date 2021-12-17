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

            return Ok(CreatedAtAction(nameof(GetExpense), new { id = expense.ExpenseDetails }, expense.AsDto()));
        }

        // DELETE /expense/{expenseId}
        [HttpDelete("expenseId")]
        public ActionResult DeleteExpense(int expenseId)
        {
            var existingExpense = iExpensesRepository.GetExpense(expenseId);

            if (existingExpense is null)
            {
                return NotFound();
            }

            iExpensesRepository.DeleteExpense(existingExpense.ExpenseId);

            return NoContent();
        }

        [HttpGet("{expenseId}")]
        public ActionResult<ExpenseDto> GetExpense(int expenseId)
        {
            var expense = iExpensesRepository.GetExpense(expenseId);

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
        [HttpPut("{expenseId}")]
        public ActionResult UpdateExpense(int expenseId, UpdateExpenseDto updateExpenseDto)
        {
            var existingExpense = iExpensesRepository.GetExpense(expenseId);

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