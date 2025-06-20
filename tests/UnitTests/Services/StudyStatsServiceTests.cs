using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Services;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Services
{
    public class StudyStatsServiceTests
    {
        private ApplicationDbContext CreateDbContextWithEntries(IEnumerable<StudyEntry> entries)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "StudyStatsTestDb_" + System.Guid.NewGuid())
                .Options;

            var context = new ApplicationDbContext(options);
            context.StudyEntries.AddRange(entries);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task CalculateStatsAsync_ReturnsEmptyResult_WhenNoEntries()
        {
            var context = CreateDbContextWithEntries(new List<StudyEntry>());
            var logger = new Mock<ILogger<StudyStatsService>>();
            var service = new StudyStatsService(context, logger.Object);

            var result = await service.CalculateStatsAsync();

            Assert.NotNull(result);
            Assert.Equal(0, result.TotalMinutes);
            Assert.Equal(0, result.AverageDuration);
            Assert.Empty(result.PerSubject);
        }

        [Fact]
        public async Task CalculateStatsAsync_ReturnsCorrectStats_WhenEntriesExist()
        {
            var entries = new List<StudyEntry>
            {
                new StudyEntry { Subject = "Math", DurationInMinutes = 60 },
                new StudyEntry { Subject = "Math", DurationInMinutes = 30 },
                new StudyEntry { Subject = "Science", DurationInMinutes = 45 }
            };

            var context = CreateDbContextWithEntries(entries);
            var logger = new Mock<ILogger<StudyStatsService>>();
            var service = new StudyStatsService(context, logger.Object);

            var result = await service.CalculateStatsAsync();

            Assert.Equal(135, result.TotalMinutes);
            Assert.Equal(45, result.AverageDuration);
            Assert.Equal(2, result.PerSubject.Count);

            var mathStats = result.PerSubject.FirstOrDefault(s => s.Subject == "Math");
            Assert.NotNull(mathStats);
            Assert.Equal(2, mathStats.SessionCount);
            Assert.Equal(45, mathStats.AverageDuration);

            var scienceStats = result.PerSubject.FirstOrDefault(s => s.Subject == "Science");
            Assert.NotNull(scienceStats);
            Assert.Equal(1, scienceStats.SessionCount);
            Assert.Equal(45, scienceStats.AverageDuration);
        }
    }
}
