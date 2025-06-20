using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using StudyTracker.Data;
using StudyTracker.Models;

namespace StudyTracker.Services
{
    public class StudyExportService
    {
        private readonly StudyDbContext _context;
        private readonly ILogger<StudyExportService> _logger;

        public StudyExportService(StudyDbContext context, ILogger<StudyExportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string ExportToCsv()
        {
            var entries = _context.StudyEntries.ToList();

            if (!entries.Any())
            {
                _logger.LogWarning("No study entries found to export.");
                return string.Empty;
            }

            var csv = new StringBuilder();
            csv.AppendLine("Id,Subject,DurationInMinutes,Date");

            foreach (var entry in entries)
            {
                csv.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0},\"{1}\",{2},{3}",
                    entry.Id,
                    entry.Subject,
                    entry.DurationInMinutes,
                    entry.Date.ToString("yyyy-MM-dd")));
            }

            _logger.LogInformation("Exported {Count} study entries to CSV.", entries.Count);
            return csv.ToString();
        }
    }
}