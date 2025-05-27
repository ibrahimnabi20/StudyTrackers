using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using StudyTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public class StudyService : IStudyService
    {
        private readonly StudyDbContext _context;

        public StudyService(StudyDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudyEntry>> GetAllAsync() =>
            await _context.StudyEntries.ToListAsync();

        public async Task<StudyEntry?> GetByIdAsync(int id) =>
            await _context.StudyEntries.FindAsync(id);

        public async Task<StudyEntry> CreateAsync(StudyEntry entry)
        {
            if (entry.DurationInMinutes <= 0)
                throw new ArgumentException("Duration must be greater than 0");

            _context.StudyEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _context.StudyEntries.FindAsync(id);
            if (entry == null) return false;

            _context.StudyEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
