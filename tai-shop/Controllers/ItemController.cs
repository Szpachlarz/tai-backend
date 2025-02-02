using Microsoft.AspNetCore.Mvc;
using tai_shop.Data;
using tai_shop.Dtos.Item;
using tai_shop.Interfaces;
using tai_shop.Mappers;
using tai_shop.Models;
using tai_shop.Services;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemRepository _itemRepository;
        private readonly IPhotoService _photoService;

        public ItemController(ApplicationDbContext context, IItemRepository itemRepository, IPhotoService photoService)
        {
            _context = context;
            _itemRepository = itemRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var items = await _itemRepository.GetAllAsync();

            var itemDto = items.Select(s => s.ToItemDto()).ToList();

            return Ok(itemDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item.ToItemDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateItemDto itemDto, [FromForm] IEnumerable<IFormFile> files)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemModel = itemDto.ToItemFromCreateDto();

            await _itemRepository.CreateAsync(itemModel);

            if (files != null && files.Any())
            {
                var photos = await _photoService.Upload(itemModel, files);
                itemModel.Photos.AddRange(photos);
            }

            return CreatedAtAction(nameof(GetById), new { id = itemModel.Id }, itemModel.ToItemDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemModel = await _itemRepository.DeleteAsync(id);

            if (itemModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateItemDto updateDto, [FromForm] IEnumerable<IFormFile> photos, [FromForm] List<int> photosToDelete)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _itemRepository.GetByIdAsync(id);

            if (photosToDelete != null && photosToDelete.Any())
            {
                var photosToRemove = item.Photos.Where(p => photosToDelete.Contains(p.Id)).ToList();
                foreach (var photo in photosToRemove)
                {
                    _photoService.Delete(photo.Filename);
                    item.Photos.Remove(photo);
                }
            }

            if (photos != null && photos.Any())
            {
                var uploadedPhotos = await _photoService.Upload(item, photos);
                item.Photos.AddRange(uploadedPhotos);
            }

            var itemModel = await _itemRepository.UpdateAsync(id, updateDto);

            if (itemModel == null)
            {
                return NotFound();
            }

            return Ok(itemModel.ToItemDto());
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStockQuantity([FromRoute]int id, [FromBody] UpdateStockQuantityDto updateStockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _itemRepository.UpdateStockQuantityAsync(id, updateStockDto);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
