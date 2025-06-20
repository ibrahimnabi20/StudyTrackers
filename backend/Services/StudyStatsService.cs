using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudyTracker.Data;
using StudyTracker.Models;

namespace StudyTracker.Services
{
    // Hvis du ønsker et interface, tilføj det:
    public interface IStudyStatsService
    {
        Task<StudyStatsResult> CalculateStatsAsync(CancellationToken cancellationToken);
    }

    public class StudyStatsService : IStudyStatsService
    {
        private readonly StudyDbContext _context;
        private readonly ILogger<StudyStatsService> _logger;

        public StudyStatsService(StudyDbContext context, ILogger<StudyStatsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<StudyStatsResult> CalculateStatsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Calculating study statistics...");

            // Hent alle entries asynkront
            var entries = await _context.StudyEntries
                                        .AsNoTracking()
                                        .ToListAsync(cancellationToken);

            if (entries.Count == 0)
            {
                _logger.LogInformation("No study entries found when calculating statistics.");
                return new StudyStatsResult();
            }

            var totalMinutes    = entries.Sum(e => e.DurationInMinutes);
            var averageDuration = entries.Average(e => e.DurationInMinutes);

            var perSubject = entries
                .GroupBy(e => e.Subject)
                .Select(g => new SubjectStats
                {
                    Subject         = g.Key,
                    SessionCount    = g.Count(),
                    AverageDuration = g.Average(e => e.DurationInMinutes)
                })
                .ToList();

            _logger.LogInformation("Statistics calculated successfully for {Count} entries.", entries.Count);

            return new StudyStatsResult
            {
                TotalMinutes    = totalMinutes,
                AverageDuration = averageDuration,
                PerSubject      = perSubject
            };
        }
    }

    public class StudyStatsResult
    {
        public int TotalMinutes { get; set; }
        public double AverageDuration { get; set; }
        public List<SubjectStats> PerSubject { get; set; } = new();
    }

    public class SubjectStats
    {
        public string Subject { get; set; } = string.Empty;
        public int SessionCount { get; set; }
        public double AverageDuration { get; set; }
    }
}
