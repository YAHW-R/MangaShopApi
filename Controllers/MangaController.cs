using MangaShopApi.Services;
using MangaShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MangaShopApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MangaController : ControllerBase
    {
        private readonly ILogger<MangaController> _logger;
        private readonly IMangaService _mangaService;


        public MangaController(ILogger<MangaController> logger, IMangaService mangaService)
        {
            _logger = logger;
            _mangaService = mangaService;
        }

        [HttpGet]
        public async Task<IEnumerable<Manga>> Get()
        {
            return await _mangaService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Manga>> GetById(string id)
        {
            var manga = await _mangaService.GetAsync(id);
            if (manga == null)
            {
                return NotFound();
            }
            return manga;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(Manga newManga)
        {
            newManga.Id = null;
            await _mangaService.CreateAsync(newManga);
            return CreatedAtAction(nameof(Get), new { id = newManga.Id }, newManga);
        }

        [Authorize]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Manga updatedManga)
        {
            var manga = _mangaService.GetAsync(id);
            if (manga == null)
            {
                return NotFound();
            }

            updatedManga.Id = id;
            await _mangaService.UpdateAsync(id, updatedManga);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var manga = _mangaService.GetAsync(id);
            if (manga == null)
            {
                return NotFound();
            }

            await _mangaService.RemoveAsync(id);
            return NoContent();
        }
    }
}
