using tai_shop.Dtos.Return;
using tai_shop.Models;

namespace tai_shop.Interfaces
{
    public interface IReturnRepository
    {
        Task<IEnumerable<Return>> GetAllReturnsAsync();
        Task<ReturnDto> CreateReturnRequest(CreateReturnDto returnDto, string userId);
        Task<ReturnDto> GetReturnRequest(int id, string userId);
        Task<Return> ApproveReturn(int returnId);
        Task<Return> RejectReturn(int returnId, string reason);
        Task<Return> ProcessReturn(int returnId);
        Task<decimal> RefundReturn(int returnId);
    }
}
