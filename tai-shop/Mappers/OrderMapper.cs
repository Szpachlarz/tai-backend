using tai_shop.Dtos.Order;
using tai_shop.Enums;
using tai_shop.Models;

namespace tai_shop.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order order)
        {
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShippingMethod = order.ShippingMethod,
                TotalAmount = order.TotalAmount,
                Items = order.ItemOrders?.Select(io => new OrderItemDto
                {
                    ItemId = io.ItemId,
                    Quantity = io.Quantity,
                    UnitPrice = io.Item.Price,
                    Subtotal = io.Quantity * io.Item.Price
                }).ToList() ?? new List<OrderItemDto>()
            };
        }

        public static Order ToEntity(this OrderDto dto)
        {
            if (dto == null) return null;

            return new Order
            {
                Id = dto.Id,
                UserId = dto.UserId,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                ShippingMethod = dto.ShippingMethod,
                ItemOrders = dto.Items?.Select(item => new ItemOrder
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    Price = item.UnitPrice
                }).ToList() ?? new List<ItemOrder>()
            };
        }

        //public static Order ToEntity(this CreateOrderDto dto, string userId, IEnumerable<Item> items)
        //{
        //    if (dto == null) return null;

        //    var itemsDict = items.ToDictionary(i => i.Id, i => i.Price);

        //    var itemOrders = dto.Items?.Select(item => new ItemOrder
        //    {
        //        ItemId = item.ItemId,
        //        Quantity = item.Quantity,
        //        Price = itemsDict.GetValueOrDefault(item.ItemId)
        //    }).ToList() ?? new List<ItemOrder>();

        //    return new Order
        //    {
        //        UserId = userId,
        //        OrderDate = DateTime.UtcNow,
        //        Status = OrderStatus.Pending,
        //        ShippingMethod = dto.ShippingMethod,
        //        ItemOrders = itemOrders
        //    };
        //}
    }
}
