using Microsoft.AspNetCore.Mvc;
using StudyTracker.Models;
using StudyTracker.Services;
using System.Threading.Tasks;

namespace StudyTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyController : ControllerBase
    {
        private readonly IStudyService _service;

        public StudyController(IStudyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _service.GetByIdAsync(id);
            return entry == null ? NotFound() : Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudyEntry entry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(entry);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}