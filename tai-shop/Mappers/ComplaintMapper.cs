using tai_shop.Dtos.Complaint;
using tai_shop.Enums;
using tai_shop.Models;

namespace tai_shop.Mappers
{
    public static class ComplaintMapper
    {
        public static ComplaintDto ToComplaintDto(this CustomerComplaint complaint)
        {
            return new ComplaintDto
            {
                Id = complaint.Id,
                OrderId = complaint.OrderId ?? 0,
                Description = complaint.Description,
                Status = complaint.Status,
                CreatedDate = complaint.CreatedDate,
                LastModifiedDate = complaint.LastModifiedDate,
                ResolvedDate = complaint.ResolvedDate
            };
        }

        public static IEnumerable<ComplaintDto> ToComplaintDtos(this IEnumerable<CustomerComplaint> complaints)
        {
            return complaints.Select(c => c.ToComplaintDto());
        }

        public static CustomerComplaint ToComplaintFromCreateDto(this CreateComplaintDto createDto)
        {
            return new CustomerComplaint
            {
                OrderId = createDto.OrderId,
                Description = createDto.Description,
                Status = ComplaintStatus.New
            };
        }

        public static void UpdateFromDto(this CustomerComplaint complaint, UpdateComplaintDto updateDto)
        {
            if (!string.IsNullOrEmpty(updateDto.Description))
            {
                complaint.Description = updateDto.Description;
            }

            if (updateDto.Status.HasValue)
            {
                complaint.Status = updateDto.Status.Value;
            }
        }
    }
}
