using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            this.expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseListDto>>> GetAllExpenses()
        {
            var fullExpenses = await expenseService.GetAllExpensesAsync();
            return Ok(fullExpenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseListDto>> GetExpenseById(int id, bool includeCategory = false, bool includePaymentMethod = false)
        {
            var expense = await expenseService.GetExpenseByIdAsync(id, includeCategory, includePaymentMethod);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseListDto>> AddExpenseAsync([FromBody] CreateExpenseDto expenseCreateDto)
        {
            if (expenseCreateDto == null)
            {
                return BadRequest();
            }

            var createdExpense = await expenseService.AddExpenseAsync(expenseCreateDto);

            return CreatedAtAction(
                nameof(GetExpenseById),
                new { id = createdExpense.Id },
                createdExpense);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpenseAsync(int id)
        {
            var isDeleted = await expenseService.DeleteExpenseAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
