using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using StudyTracker.Models;
using StudyTracker.Services;

namespace StudyTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IStudyExportService _exportService;
        private readonly FeatureToggles _toggles;
        private readonly ILogger<ExportController> _logger;

        // 3-param constructor for DI
        public ExportController(
            IStudyExportService exportService,
            IOptions<FeatureToggles> featureToggles,
            ILogger<ExportController> logger)
        {
            _exportService = exportService;
            _toggles       = featureToggles.Value;
            _logger        = logger;
        }

        // 2-param constructor (service + logger) — used by your existing tests
        public ExportController(
            IStudyExportService exportService,
            ILogger<ExportController> logger)
            : this(
                exportService,
                Options.Create(new FeatureToggles { EnableAdvancedExport = true }),
                logger)
        { }

        // 1-param constructor (service only) — if any tests rely on this
        public ExportController(IStudyExportService exportService)
            : this(
                exportService,
                Options.Create(new FeatureToggles { EnableAdvancedExport = true }),
                NullLogger<ExportController>.Instance)
        { }

        [HttpGet]
        public IActionResult GetExport()
        {
            if (!_toggles.EnableAdvancedExport)
            {
                _logger.LogWarning("Advanced export feature is disabled.");
                return BadRequest("Export feature is disabled.");
            }

            var csvBytes = _exportService.ExportToCsv();
            if (csvBytes == null || csvBytes.Length == 0)
            {
                _logger.LogInformation("No data available for export.");
                return NoContent();
            }

            var fileName = $"study-entries_{DateTime.UtcNow:yyyyMMdd}.csv";
            return File(csvBytes, "text/csv", fileName);
        }
    }
}
