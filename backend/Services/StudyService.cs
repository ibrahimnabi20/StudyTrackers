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
        private readonly IOptions<FeatureToggles> _featureToggles;

        // This constructor takes the database context, logger, and feature toggle settings.
        public StudyService(StudyDbContext context, ILogger<StudyService> logger, IOptions<FeatureToggles> featureToggles)
        {
            _context = context;
            _logger = logger;
            _featureToggles = featureToggles;
        }

        // Fetches all study entries from the database.
        public async Task<List<StudyEntry>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all study entries from the database");
            return await Task.FromResult(_context.StudyEntries.ToList());
        }

        // Fetches a specific study entry by ID.
        public async Task<StudyEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching study entry with ID {Id}", id);
            return await _context.StudyEntries.FindAsync(new object[] { id }, cancellationToken);
        }

        // Adds a new study entry – but only if the feature toggle allows it.
        public async Task<StudyEntry> CreateAsync(StudyEntry entry, CancellationToken cancellationToken = default)
        {
            // Feature toggle check to allow/disallow creating entries
            if (!_featureToggles.Value.EnableCreateEntry)
            {
                _logger.LogWarning("Attempt to create entry while feature toggle is disabled");
                throw new InvalidOperationException("Creating entries is currently disabled.");
            }

            _logger.LogInformation("Creating a new study entry for subject {Subject}", entry.Subject);
            _context.StudyEntries.Add(entry);
            await _context.SaveChangesAsync(cancellationToken);
            return entry;
        }

        // Deletes a study entry by ID.
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entry = await _context.StudyEntries.FindAsync(new object[] { id }, cancellationToken);
            if (entry == null)
            {
                // Logs a warning if the entry does not exist
                _logger.LogWarning("Attempted to delete non-existent entry with ID {Id}", id);
                return false;
            }

            _logger.LogInformation("Deleting study entry with ID {Id}", id);
            _context.StudyEntries.Remove(entry);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Updates an existing study entry with new data.
        public async Task<bool> UpdateAsync(StudyEntry updatedEntry, CancellationToken cancellationToken = default)
        {
            var entry = await _context.StudyEntries.FindAsync(new object[] { updatedEntry.Id }, cancellationToken);
            if (entry == null)
            {
                // If the entry doesn’t exist, we log it and return false
                _logger.LogWarning("Attempted to update non-existent entry with ID {Id}", updatedEntry.Id);
                return false;
            }

            // Updating each field
            entry.Subject = updatedEntry.Subject;
            entry.DurationInMinutes = updatedEntry.DurationInMinutes;
            entry.Timestamp = updatedEntry.Timestamp;

            _logger.LogInformation("Updating study entry with ID {Id}", updatedEntry.Id);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
