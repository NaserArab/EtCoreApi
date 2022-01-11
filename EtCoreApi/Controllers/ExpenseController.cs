using System;
using System.Collections;
using EtCoreApi.Dtos;
using EtCoreApi.Entities;
using EtCoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EtCoreApi.Controllers
{
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpensesRepository iExpensesRepository;
        private readonly ILogger<ExpenseController> logger;
        private readonly IMapper mapper;

        public ExpenseController(IExpensesRepository _iExpensesRepository, ILogger<ExpenseController> logger,IMapper mapper)
        {
            this.iExpensesRepository = _iExpensesRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("Expense/CreateExpenseAsync")]
        public async Task<ActionResult<ExpenseCreateDto>> CreateExpenseAsync(ExpenseCreateDto createExpenseDto)
        {
            var expense = mapper.Map<Expense>(createExpenseDto);

            await iExpensesRepository.CreateExpenseAsync(expense);
            await iExpensesRepository.SaveChanges();

            var expenseReadDto = mapper.Map<ExpenseReadDto>(expense);

            return Ok(CreatedAtRoute(nameof(GetExpenseByIdAsync), new { id = expenseReadDto.Id }, expenseReadDto));
        }

        // DELETE /expense/{Id}
        [HttpDelete]
        [Route("Expense/DeleteExpenseAsync/{Id:Guid}")]
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
        //[HttpGet("{details}")]
        //public async Task<IEnumerable<ExpenseReadDto>> GetExpensesByDetailsAsync(string details = null)
        //{
        //    var expenses = (await iExpensesRepository.GetExpensesAsync()).Select(expense => expense.AsDto());

        //    if (string.IsNullOrWhiteSpace(details) == false)
        //    {
        //        expenses = expenses.Where(item => item.ExpenseDetails.Contains(details, StringComparison.OrdinalIgnoreCase));
        //    }

        //    logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Retrieved {expenses.Count()} items");

        //    return expenses;
        //}

        [HttpGet]
        [Route("Expense/GetExpenseByIdAsync/{Id:Guid}")]
        public async Task<ActionResult<ExpenseReadDto>> GetExpenseByIdAsync(Guid Id)
        {
            var expense = await iExpensesRepository.GetExpenseAsync(Id);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ExpenseReadDto>(expense));
        }

        [HttpGet]
        [Route("Expense/GetExpensesAsync")]
        public async Task<ActionResult<IEnumerable<ExpenseReadDto>>> GetExpensesAsync()
        {
            var expenses = await iExpensesRepository.GetExpensesAsync();

            var expensesReadDtos = mapper.Map<IEnumerable<ExpenseReadDto>>(expenses);

            return Ok(expensesReadDtos);
        }

        // PUT /expense/{id}
        [HttpPut]
        [Route("Expense/UpdateExpenseAsync/{Id:Guid}")]
        public async Task<ActionResult> UpdateExpenseAsync(Guid Id, ExpenseUpdateDto updateExpenseDto)
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