using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<TransactionListDto>>> GetTransactions([FromQuery] TransactionFilterDto filters)
        {
            var result = await transactionService.GetAllTransactionsAsync(filters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionListDto>> GetTransactionById(int id)
        {
            var transaction = await transactionService.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionListDto>> AddTransactionAsync([FromBody] CreateTransactionDto createTransactionDto)
        {
            if (createTransactionDto == null)
            {
                return BadRequest();
            }

            var createdTransaction = await transactionService.AddTransactionAsync(createTransactionDto);

            return CreatedAtAction(
                nameof(GetTransactionById),
                new { id = createdTransaction.Id },
                createdTransaction);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionListDto>> UpdateTransactionAsync(
            int id,
            [FromBody] UpdateTransactionDto updateTransactionDto)
        {
            if (updateTransactionDto == null)
            {
                return BadRequest();
            }

            var updatedTransaction = await transactionService.UpdateTransactionAsync(id, updateTransactionDto);

            if (updatedTransaction == null)
            {
                return NotFound();
            }

            return Ok(updatedTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransactionAsync(int id)
        {
            var isDeleted = await transactionService.DeleteTransactionAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
