using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudyTracker.Models;
using StudyTracker.Services;

namespace StudyTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyController : ControllerBase
    {
        private readonly IStudyService _studyService;
        private readonly ILogger<StudyController> _logger;
        private readonly IOptions<FeatureToggles> _featureToggles;

        public StudyController(
            IStudyService studyService,
            ILogger<StudyController> logger,
            IOptions<FeatureToggles> featureToggles)
        {
            _studyService = studyService;
            _logger = logger;
            _featureToggles = featureToggles;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudyEntry>>> GetAll(CancellationToken cancellationToken)
        {
            var entries = await _studyService.GetAllAsync(cancellationToken);
            return Ok(entries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudyEntry>> GetById(int id, CancellationToken cancellationToken)
        {
            var entry = await _studyService.GetByIdAsync(id, cancellationToken);
            if (entry == null)
            {
                return NotFound();
            }

            return Ok(entry);
        }

        [HttpPost]
        public async Task<ActionResult<StudyEntry>> Create(StudyEntry entry, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _studyService.CreateAsync(entry, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Create entry failed due to disabled feature toggle");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var success = await _studyService.DeleteAsync(id, cancellationToken);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudyEntry updatedEntry, CancellationToken cancellationToken)
        {
            if (id != updatedEntry.Id)
            {
                return BadRequest();
            }

            var success = await _studyService.UpdateAsync(updatedEntry, cancellationToken);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("/api/feature-toggles")]
        public IActionResult GetFeatureToggles()
        {
            return Ok(_featureToggles.Value);
        }
    }
}
