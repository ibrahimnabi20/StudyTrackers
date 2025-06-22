using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StudyTracker.Models;
using StudyTracker.Services;

namespace StudyTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IStudyStatsService _statsService;
        private readonly IOptions<FeatureToggles> _featureToggles;

        public StatsController(IStudyStatsService statsService, IOptions<FeatureToggles> featureToggles)
        {
            _statsService = statsService;
            _featureToggles = featureToggles;
        }

        [HttpGet]
        public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
        {
            if (!_featureToggles.Value.EnableStudyStats)
            {
                return NotFound("Statistics disabled.");
            }

            var stats = await _statsService.CalculateStatsAsync(cancellationToken);
            return Ok(stats);
        }
    }
}