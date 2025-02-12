using tai_shop.Dtos.Return;
using tai_shop.Models;

namespace tai_shop.Mappers
{
    public static class ReturnMapper
    {
        public static ReturnDto ToDto(this Return returnEntity)
        {
            if (returnEntity == null) return null;
            return new ReturnDto
            {
                Id = returnEntity.Id,
                OrderId = returnEntity.OrderId,
                ReturnRequestDate = returnEntity.ReturnRequestDate,
                ProcessedDate = returnEntity.ProcessedDate,
                Status = returnEntity.Status,
                Reason = returnEntity.Reason,
                CustomerNotes = returnEntity.CustomerNotes,
                RefundAmount = returnEntity.RefundAmount,
                Order = returnEntity.Order?.ToDto(),
                ReturnItems = returnEntity.ItemReturns?.Select(ir => new ReturnItemDto
                {
                    Id = ir.Id,
                    ItemOrderId = ir.ItemOrderId,
                    QuantityToReturn = ir.Quantity,
                    ItemName = ir.ItemOrder?.Item?.Name,
                    Price = ir.ItemOrder?.Price ?? 0,
                    //ProductImageUrl = ir.ItemOrder?.Item?.ImageUrl
                }).ToList() ?? new List<ReturnItemDto>()
            };
        }

        public static Return ToEntity(this ReturnDto dto)
        {
            if (dto == null) return null;
            return new Return
            {
                Id = dto.Id,
                OrderId = dto.OrderId,
                ReturnRequestDate = dto.ReturnRequestDate,
                ProcessedDate = dto.ProcessedDate,
                Status = dto.Status,
                Reason = dto.Reason,
                CustomerNotes = dto.CustomerNotes,
                RefundAmount = dto.RefundAmount,
                Order = dto.Order?.ToEntity(),
                ItemReturns = dto.ReturnItems?.Select(item => new ItemReturn
                {
                    Id = item.Id,
                    ItemOrderId = item.ItemOrderId,
                    Quantity = item.QuantityToReturn,
                    ReturnId = dto.Id
                }).ToList() ?? new List<ItemReturn>()
            };
        }
    }
}
