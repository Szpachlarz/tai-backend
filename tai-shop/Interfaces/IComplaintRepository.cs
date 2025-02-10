using tai_shop.Enums;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IComplaintRepository
    {
        Task<CustomerComplaint> GetByIdAsync(int id);
        Task<IEnumerable<CustomerComplaint>> GetAllAsync();
        Task<IEnumerable<CustomerComplaint>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<CustomerComplaint>> GetOpenComplaintsAsync();
        Task<IEnumerable<CustomerComplaint>> GetComplaintsByStatusAsync(ComplaintStatus status);
        Task<CustomerComplaint> CreateAsync(CustomerComplaint complaint);
        Task<CustomerComplaint> UpdateAsync(int complaintId, CustomerComplaint complaint);
        Task<bool> DeleteAsync(int id);
        Task<int> GetOpenComplaintsCountAsync();
    }
}
