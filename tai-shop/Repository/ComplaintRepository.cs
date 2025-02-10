using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
using tai_shop.Enums;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Repository
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly ApplicationDbContext _context;
        public ComplaintRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerComplaint> GetByIdAsync(int id)
        {
            return await _context.Complaints
                .Include(c => c.Order)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CustomerComplaint>> GetAllAsync()
        {
            return await _context.Complaints
                .Include(c => c.Order)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerComplaint>> GetByCustomerIdAsync(string customerId)
        {
            return await _context.Complaints                
                .Include(c => c.Order)
                .Where(c => c.Order.UserId == customerId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerComplaint>> GetOpenComplaintsAsync()
        {
            return await _context.Complaints
                .Where(c => c.Status != ComplaintStatus.Resolved &&
                           c.Status != ComplaintStatus.Closed)
                .Include(c => c.Order)
                .OrderBy(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerComplaint>> GetComplaintsByStatusAsync(ComplaintStatus status)
        {
            return await _context.Complaints
                .Where(c => c.Status == status)

                .Include(c => c.Order)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<CustomerComplaint> CreateAsync(CustomerComplaint complaint)
        {

            complaint.CreatedDate = DateTime.UtcNow;
            complaint.LastModifiedDate = DateTime.UtcNow;

            await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();

            return complaint;
        }

        public async Task<CustomerComplaint?> UpdateAsync(int complaintId, CustomerComplaint complaint)
        {
            var existingComplaint = await _context.Complaints.FirstOrDefaultAsync(x => x.Id == complaintId);

            if (existingComplaint == null)
            {
                return null;
            }

            existingComplaint.LastModifiedDate = DateTime.UtcNow;

            if (complaint.Status == ComplaintStatus.Resolved || complaint.Status == ComplaintStatus.Closed)
            {
                existingComplaint.ResolvedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return existingComplaint;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null) return false;

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetOpenComplaintsCountAsync()
        {
            return await _context.Complaints
                .CountAsync(c => c.Status != ComplaintStatus.Resolved &&
                               c.Status != ComplaintStatus.Closed);
        }
    }
}
