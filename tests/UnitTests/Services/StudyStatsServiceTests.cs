using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Services;
using Xunit;

namespace UnitTests.Services
{
    public class StudyStatsServiceTests
    {
        private StudyDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<StudyDbContext>()
                .UseInMemoryDatabase("StatsTestDb_" + Guid.NewGuid())
                .Options;
            return new StudyDbContext(options);
        }

        private StudyStatsService CreateServiceWithEntries(List<StudyEntry> entries)
        {
            var context = CreateInMemoryDbContext();
            context.StudyEntries.AddRange(entries);
            context.SaveChanges();

            var logger = new Mock<ILogger<StudyStatsService>>();
            return new StudyStatsService(context, logger.Object);
        }

        [Fact]
        public async Task CalculateStatsAsync_ReturnsZeroStats_WhenNoEntries()
        {
            var service = CreateServiceWithEntries(new List<StudyEntry>());

            var result = await service.CalculateStatsAsync(CancellationToken.None);

            Assert.Equal(0, result.TotalMinutes);
            Assert.Equal(0, result.AverageDuration);
            Assert.Empty(result.PerSubject);
        }

        [Fact]
        public async Task CalculateStatsAsync_ComputesCorrectTotals_AndLogs()
        {
            var entries = new List<StudyEntry>
            {
                new StudyEntry { Id = 1, Subject = "Math", DurationInMinutes = 30 },
                new StudyEntry { Id = 2, Subject = "Math", DurationInMinutes = 60 },
                new StudyEntry { Id = 3, Subject = "History", DurationInMinutes = 45 }
            };
            var mockLogger = new Mock<ILogger<StudyStatsService>>();
            var context    = CreateInMemoryDbContext();
            context.StudyEntries.AddRange(entries);
            context.SaveChanges();
            var service    = new StudyStatsService(context, mockLogger.Object);

            var result = await service.CalculateStatsAsync(CancellationToken.None);

            // Total and average
            Assert.Equal(135, result.TotalMinutes);
            Assert.Equal(45, result.AverageDuration);

            // Per‐subject grouping
            Assert.Collection(result.PerSubject,
                math =>
                {
                    Assert.Equal("Math", math.Subject);
                    Assert.Equal(2, math.SessionCount);
                    Assert.Equal(45, math.AverageDuration);
                },
                hist =>
                {
                    Assert.Equal("History", hist.Subject);
                    Assert.Equal(1, hist.SessionCount);
                    Assert.Equal(45, hist.AverageDuration);
                });

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Statistics calculated successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
