using Microsoft.AspNetCore.Mvc;
using tai_shop.Data;
using tai_shop.Interfaces;
using tai_shop.Mappers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemRepository _itemRepository; 

        public ItemController(ApplicationDbContext context, IItemRepository itemRepository)
        {
            _context = context;
            _itemRepository = itemRepository;
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
    }
}
