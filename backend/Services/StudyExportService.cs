// Services/StudyExportService.cs
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using StudyTracker.Data;
using StudyTracker.Models;

namespace StudyTracker.Services
{
    public class StudyExportService : IStudyExportService
    {
        private readonly StudyDbContext _context;
        private readonly ILogger<StudyExportService> _logger;

        public StudyExportService(StudyDbContext context, ILogger<StudyExportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public byte[] ExportToCsv()
        {
            var entries = _context.StudyEntries.ToList();

            if (!entries.Any())
            {
                _logger.LogWarning("No study entries found to export.");
                return Array.Empty<byte>();
            }

            var csv = new StringBuilder();
            csv.AppendLine("Id,Subject,DurationInMinutes,Timestamp");

            foreach (var e in entries)
            {
                csv.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0},\"{1}\",{2},{3}",
                    e.Id,
                    e.Subject,
                    e.DurationInMinutes,
                    e.Timestamp.ToString("s")));
            }

            _logger.LogInformation("Exported {Count} study entries to CSV.", entries.Count);
            return Encoding.UTF8.GetBytes(csv.ToString());
        }
    }
}