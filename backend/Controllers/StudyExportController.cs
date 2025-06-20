using Microsoft.AspNetCore.Mvc;
using StudyTracker.Services;

namespace StudyTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly StudyExportService _exportService;

        public ExportController(StudyExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpGet]
        public IActionResult GetExport()
        {
            var fileBytes = _exportService.ExportToCsv();
            if (fileBytes.Length == 0)
            {
                return NotFound("No data available to export.");
            }

            return File(fileBytes, "text/csv", "study-entries.csv");
        }
    }
}