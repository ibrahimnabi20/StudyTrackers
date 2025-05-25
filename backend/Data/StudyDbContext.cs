using Microsoft.EntityFrameworkCore;
using StudyTracker.Models;

namespace StudyTracker.Data
{
    public class StudyDbContext : DbContext
    {
        public StudyDbContext(DbContextOptions<StudyDbContext> options)
            : base(options) { }

        public DbSet<StudyEntry> StudyEntries { get; set; }
    }
}