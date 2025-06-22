using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly FeatureToggles _featureToggles;
        private readonly ILogger<ExportController> _logger;

        public ExportController(
            IStudyExportService exportService,
            IOptions<FeatureToggles> featureToggles,
            ILogger<ExportController> logger)
        {
            _exportService = exportService;
            _featureToggles = featureToggles.Value;
            _logger = logger;
        }

        [HttpGet("export")]
        public IActionResult GetExport()
        {
            if (!_featureToggles.EnableAdvancedExport)
            {
                _logger.LogWarning("Advanced export feature is disabled.");
                return Forbid();
            }

            var csvBytes = _exportService.ExportToCsv();
            if (csvBytes == null || csvBytes.Length == 0)
            {
                _logger.LogInformation("No data available for export.");
                return NoContent();
            }

            // Denne log fanger testen, der forventer "Exported CSV"
            _logger.LogInformation("Exported CSV");

            var fileName = $"study-entries_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            return File(csvBytes, "text/csv", fileName);
        }
    }
}