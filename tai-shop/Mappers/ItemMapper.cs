using tai_shop.Dtos.Account;
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
                Rating = itemModel.AverageRating,
                Photos = itemModel.Photos?.Select(p => new PhotoDto
                {
                    Id = p.Id,
                    Filename = p.Filename,
                    Filepath = p.Filepath,
                    Length = p.Length
                }).ToList()
            };
        }

        public static Item ToItemFromCreateDto(this CreateItemDto requestDto)
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
