using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StudyTracker.Data;
using StudyTracker.Models;

namespace StudyTracker.Services
{
    public class StudyStatsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudyStatsService> _logger;

        public StudyStatsService(ApplicationDbContext context, ILogger<StudyStatsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<StudyStatsResult> CalculateStatsAsync()
        {
            var entries = _context.StudyEntries.ToList();

            if (!entries.Any())
            {
                _logger.LogInformation("No study entries found when calculating statistics.");
                return Task.FromResult(new StudyStatsResult());
            }

            var totalMinutes = entries.Sum(e => e.DurationInMinutes);
            var averageDuration = entries.Average(e => e.DurationInMinutes);

            var perSubject = entries
                .GroupBy(e => e.Subject)
                .Select(g => new SubjectStats
                {
                    Subject = g.Key,
                    SessionCount = g.Count(),
                    AverageDuration = g.Average(e => e.DurationInMinutes)
                }).ToList();

            _logger.LogInformation("Statistics calculated successfully for {Count} entries.", entries.Count);

            return Task.FromResult(new StudyStatsResult
            {
                TotalMinutes = totalMinutes,
                AverageDuration = averageDuration,
                PerSubject = perSubject
            });
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
