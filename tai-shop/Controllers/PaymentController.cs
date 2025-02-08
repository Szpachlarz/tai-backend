using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tai_shop.Dtos.Payment;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Payment>> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                var payment = await _paymentRepository.ProcessPayment(paymentDto);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{orderId}/refund")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Payment>> RefundPayment(int orderId)
        {
            try
            {
                var payment = await _paymentRepository.RefundPayment(orderId);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("order/{orderId}")]
        [Authorize]
        public async Task<ActionResult<Payment>> GetPaymentByOrder(int orderId)
        {
            var payment = await _paymentRepository.GetPaymentByOrderId(orderId);
            if (payment == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (payment.Order.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            return Ok(payment);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<List<Payment>>> GetUserPayments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payments = await _paymentRepository.GetUserPayments(userId);
            return Ok(payments);
        }
    }
}
