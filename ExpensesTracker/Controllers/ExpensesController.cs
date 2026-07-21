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
        public async Task<ActionResult<PagedResultDto<ExpenseListDto>>> GetExpenses([FromQuery] ExpenseFilterDto filters)
        {
            var result = await expenseService.GetAllExpensesAsync(filters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseListDto>> GetExpenseById(int id)
        {
            var expense = await expenseService.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseListDto>> AddExpenseAsync([FromBody] CreateExpenseDto createExpenseDto)
        {
            if (createExpenseDto == null)
            {
                return BadRequest();
            }

            var createdExpense = await expenseService.AddExpenseAsync(createExpenseDto);

            return CreatedAtAction(
                nameof(GetExpenseById),
                new { id = createdExpense.Id },
                createdExpense);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExpenseListDto>> UpdateExpenseAsync(
            int id,
            [FromBody] UpdateExpenseDto updateExpenseDto)
        {
            if (updateExpenseDto == null)
            {
                return BadRequest();
            }

            var updatedExpense = await expenseService.UpdateExpenseAsync(id, updateExpenseDto);

            if (updatedExpense == null)
            {
                return NotFound();
            }

            return Ok(updatedExpense);
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
