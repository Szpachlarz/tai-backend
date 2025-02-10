using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tai_shop.Dtos.Complaint;
using tai_shop.Enums;
using tai_shop.Interfaces;
using tai_shop.Mappers;
using tai_shop.Models;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintRepository _complaintRepository;

        public ComplaintController(
            IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetComplaints([FromQuery] ComplaintStatus? status)
        {
            try
            {
                IEnumerable<CustomerComplaint> complaints;
                if (status.HasValue)
                {
                    complaints = await _complaintRepository.GetComplaintsByStatusAsync(status.Value);
                }
                else
                {
                    complaints = await _complaintRepository.GetAllAsync();
                }

                return Ok(complaints.ToComplaintDtos());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving complaints");
            }
        }

        [HttpGet("open")]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetOpenComplaints()
        {
            try
            {
                var complaints = await _complaintRepository.GetOpenComplaintsAsync();
                return Ok(complaints.ToComplaintDtos());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving open complaints");
            }
        }

        [HttpGet("open/count")]
        public async Task<ActionResult<int>> GetOpenComplaintsCount()
        {
            try
            {
                return Ok(await _complaintRepository.GetOpenComplaintsCountAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving open complaints count");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintDto>> GetComplaint(int id)
        {
            try
            {
                var complaint = await _complaintRepository.GetByIdAsync(id);
                if (complaint == null)
                {
                    return NotFound();
                }

                return Ok(complaint.ToComplaintDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the complaint");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ComplaintDto>> CreateComplaint(CreateComplaintDto createDto)
        {
            try
            {
                var complaint = new CustomerComplaint
                {
                    OrderId = createDto.OrderId,
                    Description = createDto.Description,
                    Status = ComplaintStatus.New
                };

                complaint = await _complaintRepository.CreateAsync(complaint);
                return CreatedAtAction(
                    nameof(GetComplaint),
                    new { id = complaint.Id },
                    complaint.ToComplaintDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the complaint");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComplaint(int id, UpdateComplaintDto updateDto)
        {
            try
            {
                var complaint = await _complaintRepository.GetByIdAsync(id);
                if (complaint == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(updateDto.Description))
                {
                    complaint.Description = updateDto.Description;
                }

                if (updateDto.Status.HasValue)
                {
                    complaint.Status = updateDto.Status.Value;
                }

                await _complaintRepository.UpdateAsync(id, complaint);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the complaint");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ComplaintDto>> UpdateComplaintStatus(int id, ComplaintStatus status)
        {
            try
            {
                var existingComplaint = await _complaintRepository.GetByIdAsync(id);
                if (existingComplaint == null)
                {
                    return NotFound();
                }

                existingComplaint.Status = status;
                var updateComplaint = await _complaintRepository.UpdateAsync(id, existingComplaint);
                return Ok(updateComplaint.ToComplaintDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the complaint status");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            try
            {
                var success = await _complaintRepository.DeleteAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the complaint");
            }
        }
    }
}
