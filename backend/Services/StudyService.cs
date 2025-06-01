using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudyTracker.Data;
using StudyTracker.Models;
using System;

namespace StudyTracker.Services
{
    public class StudyService : IStudyService
    {
        private readonly StudyDbContext _context;
        private readonly ILogger<StudyService> _logger;
        private readonly FeatureToggles _featureToggles;

        public StudyService(StudyDbContext context, ILogger<StudyService> logger, IOptions<FeatureToggles> featureToggles)
        {
            _context = context;
            _logger = logger;
            _featureToggles = featureToggles.Value;
        }

        public async Task<List<StudyEntry>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all study entries from the database.");
            return await _context.StudyEntries.ToListAsync();
        }

        public async Task<StudyEntry?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching study entry with ID {Id}.", id);
            return await _context.StudyEntries.FindAsync(id);
        }

        public async Task<StudyEntry> CreateAsync(StudyEntry entry)
        {
            if (!_featureToggles.EnableCreateEntry)
            {
                _logger.LogWarning("Attempt to create entry while feature toggle is disabled.");
                throw new InvalidOperationException("Creating entries is currently disabled.");
            }

            _logger.LogInformation("Creating a new study entry: {@Entry}", entry);
            _context.StudyEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _context.StudyEntries.FindAsync(id);
            if (entry != null)
            {
                _logger.LogInformation("Deleting study entry with ID {Id}.", id);
                _context.StudyEntries.Remove(entry);
                await _context.SaveChangesAsync();
                return true;
            }

            _logger.LogWarning("Study entry with ID {Id} not found.", id);
            return false;
        }
    }
}
