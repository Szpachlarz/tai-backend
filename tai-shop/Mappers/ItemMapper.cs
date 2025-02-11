﻿using tai_shop.Dtos.Account;
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
                Tags = itemModel.ItemTags?
                    .Where(t => t.Tag != null)
                    .Select(t => t.Tag.Name)
                    .ToList() ?? new List<string>(),
                Photos = itemModel.Photos?.Select(p => new PhotoDto
                {
                    Id = p.Id,
                    Filename = p.Filename,
                    Filepath = p.Filepath,
                    Length = p.Length
                }).ToList() ?? new List<PhotoDto>()
            };
        }

        public static Item ToItemFromCreateDto(this CreateItemDto requestDto)
        {
            return new Item
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Price = requestDto.Price,
                StockQuantity = requestDto.StockQuantity,
                ItemTags = requestDto.TagIds?.Select(tagId => new ItemTag
                {
                    TagId = tagId
                }).ToList() ?? new List<ItemTag>()
            };
        }
    }
}
