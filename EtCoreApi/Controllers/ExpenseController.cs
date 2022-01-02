using System;
using EtCoreApi.Dtos;
using EtCoreApi.Entities;
using EtCoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EtCoreApi.Controllers
{
    [ApiController]
    [Route("Expense")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpensesRepository iExpensesRepository;
        private readonly ILogger<ExpenseController> logger;

        public ExpenseController(IExpensesRepository _iExpensesRepository, ILogger<ExpenseController> logger)
        {
            this.iExpensesRepository = _iExpensesRepository;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CreateExpenseDto>> CreateExpenseAsync(CreateExpenseDto createExpenseDto)
        {
            Expense expense = createExpenseDto.AsExpense();

            await iExpensesRepository.CreateExpenseAsync(expense);

            return Ok(CreatedAtAction(nameof(GetExpenseByIdAsync), new { id = expense.Id }, expense.AsDto()));
        }

        // DELETE /expense/{Id}
        [HttpDelete("Id")]
        public async Task<ActionResult> DeleteExpenseAsync(Guid Id)
        {
            var taskOfExistingExpense = iExpensesRepository.GetExpenseAsync(Id);

            if (taskOfExistingExpense is null)
            {
                return NotFound();
            }

            await iExpensesRepository.DeleteExpenseAsync(taskOfExistingExpense.Result.Id);

            return NoContent();
        }

        // GET /items
        [HttpGet("{details}")]
        public async Task<IEnumerable<ExpenseDto>> GetExpensesByDetailsAsync(string details = null)
        {
            var expenses = (await iExpensesRepository.GetExpensesAsync()).Select(expense => expense.AsDto());

            if (string.IsNullOrWhiteSpace(details) == false)
            {
                expenses = expenses.Where(item => item.ExpenseDetails.Contains(details, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Retrieved {expenses.Count()} items");

            return expenses;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpenseByIdAsync(Guid Id)
        {
            var expense = await iExpensesRepository.GetExpenseAsync(Id);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense.AsDto());
        }

        [HttpGet]
        public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync()
        {
            var expenses = (await iExpensesRepository.GetExpensesAsync()).Select(p => p.AsDto());

            return expenses;
        }

        // PUT /expense/{id}
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateExpenseAsync(Guid Id, UpdateExpenseDto updateExpenseDto)
        {
            var taskOfExistingExpense = iExpensesRepository.GetExpenseAsync(Id);

            var existingExpense = taskOfExistingExpense.Result;

            if (taskOfExistingExpense is null)
            {
                return NotFound();
            }

            Expense updatedExpense = existingExpense with
            {
                ExpenseDetails = updateExpenseDto.ExpenseDetails,
                ExpenseAmount = updateExpenseDto.ExpenseAmount,
                ExpenseDate = updateExpenseDto.ExpenseDate
            };

            await iExpensesRepository.UpdateExpenseAsync(updatedExpense);

            return NoContent();
        }
    }
}