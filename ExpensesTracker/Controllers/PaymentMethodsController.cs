using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPaymentMethodService paymentMethodService;

        public PaymentMethodsController(
            IMapper mapper,
            IPaymentMethodService paymentMethodService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetAllPaymentMethods()
        {
            var paymentMethodsResult = await paymentMethodService.GetAllPaymentMethodsAsync();
            return Ok(this.mapper.Map<IEnumerable<PaymentMethodDto>>(paymentMethodsResult));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodDto>> GetPaymentMethodById(int id)
        {
            var paymentMethodResult = await paymentMethodService.GetPaymentMethodByIdAsync(id);

            if (paymentMethodResult == null)
            {
                return NotFound();
            }

            return Ok(this.mapper.Map<PaymentMethodDto>(paymentMethodResult));
        }

        [HttpPost]
        public async Task<ActionResult> AddPaymentMethodAsync(PaymentMethodDto paymentMethodDto)
        {
            if (paymentMethodDto == null)
            {
                return BadRequest();
            }

            var createPaymentMethodCommand = this.mapper.Map<CreatePaymentMethodCommand>(paymentMethodDto);
            var paymentMethodResult = await paymentMethodService.AddPaymentMethodAsync(createPaymentMethodCommand);

            var response = this.mapper.Map<PaymentMethodDto>(paymentMethodResult);

            return CreatedAtAction(nameof(GetPaymentMethodById), new { id = response.Id }, response);
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
