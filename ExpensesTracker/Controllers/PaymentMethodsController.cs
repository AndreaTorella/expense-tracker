using ExpensesTracker.Models;
using ExpensesTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            this.paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetAllPaymentMethods()
        {
            var paymentMethods = await paymentMethodService.GetAllPaymentMethodsAsync();
            return Ok(paymentMethods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodDto>> GetPaymentMethodById(int id)
        {
            var paymentMethod = await paymentMethodService.GetPaymentMethodByIdAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return Ok(paymentMethod);
        }

        [HttpPost]
        public async Task<ActionResult> AddPaymentMethodAsync(PaymentMethodDto paymentMethodDto)
        {
            if (paymentMethodDto == null)
            {
                return BadRequest();
            }

            var createdPaymentMethod = await paymentMethodService.AddPaymentMethodAsync(paymentMethodDto);

            return CreatedAtAction(nameof(GetPaymentMethodById), new { id = createdPaymentMethod.Id }, createdPaymentMethod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentMethodAsync(int id)
        {
            var isDeleted = await paymentMethodService.DeletePaymentMethodAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
