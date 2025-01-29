using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tai_shop.Dtos;
using tai_shop.Interfaces;
using tai_shop.Models;

namespace tai_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllTags()
        {
            var tags = await _tagRepository.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _tagRepository.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(TagDto tagDto)
        {
            var tag = await _tagRepository.CreateTagAsync(tagDto);
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Tag>> UpdateTag(int id, TagDto tagDto)
        {
            var updatedTag = await _tagRepository.UpdateTagAsync(id, tagDto);
            if (updatedTag == null)
            {
                return NotFound();
            }
            return Ok(updatedTag);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Tag>> DeleteTag(int id)
        {
            var tag = await _tagRepository.DeleteTagAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost("{tagId}/items/{itemId}")]
        public async Task<ActionResult> AddTagToProduct(int tagId, int itemId)
        {
            await _tagRepository.AddTagToProductAsync(itemId, tagId);
            return Ok();
        }

        [HttpDelete("{tagId}/items/{itemId}")]
        public async Task<ActionResult> RemoveTagFromItem(int tagId, int itemId)
        {
            await _tagRepository.RemoveTagFromItemAsync(itemId, tagId);
            return Ok();
        }

        [HttpGet("items/{itemId}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetItemTags(int itemId)
        {
            var tags = await _tagRepository.GetItemTagsAsync(itemId);
            return Ok(tags);
        }

        [HttpGet("tag/{tagName}/items")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByTag(string tagName)
        {
            var items = await _tagRepository.GetItemsByTagAsync(tagName);
            return Ok(items);
        }
    }
}
