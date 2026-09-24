using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly IMapper mapper;

        public TransactionsController(
            IMapper mapper,
            ITransactionService transactionService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<TransactionListDto>>> GetTransactions([FromQuery] TransactionFilterDto filters)
        {
            var query = this.mapper.Map<TransactionQuery>(filters);

            var result = await transactionService.GetAllTransactionsAsync(query);
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

            var createTransactionCommand = this.mapper.Map<CreateTransactionCommand>(createTransactionDto);
            var createdTransaction = await transactionService.AddTransactionAsync(createTransactionCommand);

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

            var updateTransactionCommand = this.mapper.Map<UpdateTransactionCommand>(updateTransactionDto);
            var updatedTransaction = await transactionService.UpdateTransactionAsync(id, updateTransactionCommand);

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

        [HttpGet("me")]
        public ActionResult GetCurrentUser()
        {
            var userId = User.FindFirst("sub")?.Value;
            var email = User.FindFirst("email")?.Value;

            return Ok(new
            {
                userId,
                email
            });
        }
    }
}
