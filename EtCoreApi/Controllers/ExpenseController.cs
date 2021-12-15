using System.Collections.Generic;
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
        private readonly InMemExpensesRepository inMemExpensesRepository;

        public ExpenseController()
        {
            inMemExpensesRepository = new InMemExpensesRepository();
        }

        [HttpGet]
        public IEnumerable<Expense> GetExpenses()
        {

        }
    }
}
