using Microsoft.EntityFrameworkCore;
using tai_shop.Data;
using tai_shop.Dtos.Return;
using tai_shop.Enums;
using tai_shop.Exceptions;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Repository
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly ApplicationDbContext _context;

        public ReturnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReturnDto> CreateReturnRequest(CreateReturnDto returnDto, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.ItemOrders)
                    .ThenInclude(io => io.Item)
                .Include(o => o.Returns)
                    .ThenInclude(r => r.ItemReturns)
                .FirstOrDefaultAsync(o => o.Id == returnDto.OrderId && o.UserId == userId);

            if (order == null)
                throw new ArgumentException("Order not found or not authorized");

            var existingItemReturns = order.Returns
                .SelectMany(r => r.ItemReturns)
                .Where(ir => returnDto.ReturnItems.Any(ri => ri.ItemOrderId == ir.ItemOrderId))
                .ToList();

            if (existingItemReturns.Any())
                throw new ArgumentException("A return request has already been created for some of the selected items.");

            var returnItems = returnDto.ReturnItems.Select(ri =>
            {
                var itemOrder = order.ItemOrders.FirstOrDefault(io => io.Id == ri.ItemOrderId);
                if (itemOrder == null)
                    throw new ArgumentException($"Item order {ri.ItemOrderId} not found in order");

                if (ri.QuantityToReturn > itemOrder.Quantity)
                    throw new ArgumentException($"Cannot return more items than purchased for item {ri.ItemOrderId}");

                return new ItemReturn
                {
                    ItemOrderId = ri.ItemOrderId,
                    ItemOrder = itemOrder,
                    Quantity = ri.QuantityToReturn
                };
            }).ToList();

            var returnRequest = new Return
            {
                OrderId = order.Id,
                ReturnRequestDate = DateTime.UtcNow,
                Status = ReturnStatus.Requested,
                ItemReturns = returnItems,
                Reason = returnDto.Reason,
                RefundAmount = returnItems.Sum(itemReturn => itemReturn.ItemOrder.Price * itemReturn.Quantity)
            };

            _context.Returns.Add(returnRequest);
            await _context.SaveChangesAsync();

            return await GetReturnRequest(returnRequest.Id, userId);
        }

        public async Task<ReturnDto> GetReturnRequest(int id, string userId)
        {
            var returnRequest = await _context.Returns
                .Include(r => r.Order)
                    .ThenInclude(o => o.User)
                .Include(r => r.ItemReturns)
                    .ThenInclude(ri => ri.ItemOrder)
                        .ThenInclude(io => io.Item)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (returnRequest == null)
            {
                throw new NotFoundException("Return request not found");
            }

            return new ReturnDto
            {
                Id = returnRequest.Id,
                OrderId = returnRequest.OrderId,
                ReturnRequestDate = returnRequest.ReturnRequestDate,
                ProcessedDate = returnRequest.ProcessedDate,
                Status = returnRequest.Status,
                Reason = returnRequest.Reason,
                ReturnItems = returnRequest.ItemReturns.Select(ri => new ReturnItemDto
                {
                    ItemOrderId = ri.ItemOrderId,
                    QuantityToReturn = ri.Quantity,
                    ItemName = ri.ItemOrder.Item.Name,
                    Price = ri.ItemOrder.Price,
                    //ProductImageUrl = ri.ItemOrder.Item.ImageUrl
                }).ToList(),
                RefundAmount = returnRequest.RefundAmount,
            };
        }

        public async Task<Return> ApproveReturn(int returnId)
        {
            var returnRequest = await _context.Returns
                .FirstOrDefaultAsync(r => r.Id == returnId);

            if (returnRequest == null)
                throw new ArgumentException("Return request not found");

            returnRequest.Status = ReturnStatus.Approved;
            returnRequest.ProcessedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return returnRequest;
        }

        public async Task<Return> RejectReturn(int returnId, string reason)
        {
            var returnRequest = await _context.Returns
                .FirstOrDefaultAsync(r => r.Id == returnId);

            if (returnRequest == null)
                throw new ArgumentException("Return request not found");

            returnRequest.Status = ReturnStatus.Rejected;
            returnRequest.CustomerNotes += $"\nRejection Reason: {reason}";

            await _context.SaveChangesAsync();
            return returnRequest;
        }

        public async Task<Return> ProcessReturn(int returnId)
        {
            var returnRequest = await _context.Returns
                .Include(r => r.ItemReturns)
                .ThenInclude(ri => ri.ItemOrder)
                .FirstOrDefaultAsync(r => r.Id == returnId);

            if (returnRequest == null)
                throw new ArgumentException("Return request not found");

            foreach (var returnItem in returnRequest.ItemReturns)
            {
                var product = await _context.Items
                    .FirstOrDefaultAsync(p => p.Id == returnItem.ItemOrder.ItemId);

                if (product != null)
                {
                    product.StockQuantity += returnItem.Quantity;
                }
            }

            returnRequest.Status = ReturnStatus.Processed;
            await _context.SaveChangesAsync();

            return returnRequest;
        }

        public async Task<decimal> RefundReturn(int returnId)
        {
            var returnRequest = await _context.Returns
                .FirstOrDefaultAsync(r => r.Id == returnId);

            if (returnRequest == null)
                throw new ArgumentException("Return request not found");

            if (returnRequest.Status != ReturnStatus.Processed)
                throw new InvalidOperationException("Return must be processed before refunding");

            returnRequest.Status = ReturnStatus.Refunded;
            await _context.SaveChangesAsync();

            return returnRequest.RefundAmount;
        }        
    }
}
