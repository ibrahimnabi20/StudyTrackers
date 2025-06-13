using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyTracker.Models;
using StudyTracker.Services;
using System.Threading;
using System.Threading.Tasks;

namespace StudyTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyController : ControllerBase
    {
        private readonly IStudyService _studyService;
        private readonly ILogger<StudyController> _logger;

        public StudyController(IStudyService studyService, ILogger<StudyController> logger)
        {
            _studyService = studyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var studies = await _studyService.GetAllAsync(cancellationToken);
            return Ok(studies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var entry = await _studyService.GetByIdAsync(id, cancellationToken);
            if (entry == null)
                return NotFound();
            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudyEntry entry, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _studyService.CreateAsync(entry, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudyEntry entry, CancellationToken cancellationToken = default)
        {
            var result = await _studyService.UpdateAsync(entry, cancellationToken);
            if (!result)
                return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _studyService.DeleteAsync(id, cancellationToken);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}
