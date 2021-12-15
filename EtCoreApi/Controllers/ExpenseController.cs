using System.Collections.Generic;
using System.Linq;
using EtCoreApi.Dtos;
using EtCoreApi.Entities;
using EtCoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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

        [HttpGet]
        public IEnumerable<ExpenseDto> GetExpenses()
        {
            var expenses = iExpensesRepository.GetExpenses().Select( p => p.AsDto());

            return expenses;
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

        [HttpPost]
        public ActionResult<CreateExpenseDto> CreateExpense(CreateExpenseDto createExpenseDto)
        {
            Expense expense = createExpenseDto.AsExpense();

            iExpensesRepository.CreateExpense(expense);

            return CreatedAtAction(nameof(GetExpense),new {id = expense.ExpenseDetails},expense.AsDto());
        }
    }
}
