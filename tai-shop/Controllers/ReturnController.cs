using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using tai_shop.Dtos.Order;
using tai_shop.Dtos.Return;
using tai_shop.Exceptions;
using tai_shop.Interfaces;
using tai_shop.Mappers;
using tai_shop.Models;
using tai_shop.Repository;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnRepository _returnRepository;

        public ReturnController(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllReturns()
        {
            var returns = await _returnRepository.GetAllReturnsAsync();
            var returnDtos = returns.Select(r => r.ToDto()).ToList();
            return Ok(returnDtos);
        }
        
        [HttpGet("my-returns")]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyReturns()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var returns = await _returnRepository.GetMyReturnsAsync(userId);
            var returnDtos = returns.Select(r => r.ToDto()).ToList();
            return Ok(returnDtos);
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<Return>> CreateReturnRequest([FromBody] CreateReturnDto returnRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdReturn = await _returnRepository.CreateReturnRequest(returnRequest, userId);
            return CreatedAtAction(nameof(GetReturnRequest), new { id = createdReturn.Id }, createdReturn);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<Return>> GetReturnRequest(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");

                var returnRequest = await _returnRepository.GetReturnRequest(id, userId);
                return Ok(returnRequest);
            }
            catch (NotFoundException)
            {
                throw new NotFoundException("Return request not found");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/approve")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Return>> ApproveReturn(int id)
        {
            var approvedReturn = await _returnRepository.ApproveReturn(id);
            return Ok(approvedReturn);
        }

        [HttpPut("{id}/reject")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Return>> RejectReturn(int id, [FromBody] string reason)
        {
            var rejectedReturn = await _returnRepository.RejectReturn(id, reason);
            return Ok(rejectedReturn);
        }
    }
}
