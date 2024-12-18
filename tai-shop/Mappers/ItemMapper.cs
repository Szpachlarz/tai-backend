using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using tai_shop.Dtos.Item;
using tai_shop.Models;

namespace tai_shop.Mappers
{
    public static class ItemMapper
    {
        public static ItemDto ToItemDto(this Item itemModel)
        {
            return new ItemDto
            {
                Id = itemModel.Id,
                Name = itemModel.Name,
                Description = itemModel.Description,
                Price = itemModel.Price,
                Rating = itemModel.Rating
            };
        }

        public static Item ToItemFromCreateDto(this CreateItemRequestDto requestDto)
        {
            return new Item
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Price = requestDto.Price,
            };
        }
    }
}
