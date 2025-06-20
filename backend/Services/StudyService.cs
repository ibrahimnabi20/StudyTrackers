using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudyTracker.Data;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<List<StudyEntry>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all study entries from the database");
            return await Task.FromResult(_context.StudyEntries.ToList());
        }

        public async Task<StudyEntry?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching study entry with ID {EntryId}", id);
            return await _context.StudyEntries.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<StudyEntry> CreateAsync(StudyEntry entry, CancellationToken cancellationToken)
        {
            if (!_featureToggles.EnableCreateEntry)
            {
                _logger.LogWarning("Attempt to create entry while feature toggle is disabled");
                throw new InvalidOperationException("Creating entries is currently disabled.");
            }

            _logger.LogInformation("Creating a new study entry");
            _context.StudyEntries.Add(entry);
            await _context.SaveChangesAsync(cancellationToken);
            return entry;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting study entry with ID {EntryId}", id);
            var entry = await _context.StudyEntries.FindAsync(new object[] { id }, cancellationToken);
            if (entry == null)
                return false;

            _context.StudyEntries.Remove(entry);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateAsync(StudyEntry updatedEntry, CancellationToken cancellationToken)
        {
            var existingEntry = await _context.StudyEntries.FindAsync(new object[] { updatedEntry.Id }, cancellationToken);
            if (existingEntry == null)
            {
                _logger.LogWarning("Attempted to update non-existent entry with ID {EntryId}", updatedEntry.Id);
                return false;
            }

            _logger.LogInformation("Updating study entry with ID {EntryId}", updatedEntry.Id);
            existingEntry.Subject = updatedEntry.Subject;
            existingEntry.DurationInMinutes = updatedEntry.DurationInMinutes;
            existingEntry.Timestamp = updatedEntry.Timestamp;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
