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

        [Fact]
        public async Task CalculateStatsAsync_ReturnsZeroStats_WhenNoEntries_And_LogsInfo()
        {
            // Arrange
            var context    = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<StudyStatsService>>();
            var service    = new StudyStatsService(context, mockLogger.Object);

            // Act
            var result = await service.CalculateStatsAsync(CancellationToken.None);

            // Assert
            Assert.Equal(0, result.TotalMinutes);
            Assert.Equal(0, result.AverageDuration);
            Assert.Empty(result.PerSubject);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("No study entries found")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CalculateStatsAsync_ComputesCorrectTotals_And_LogsInformation()
        {
            // Arrange
            var entries = new List<StudyEntry>
            {
                new StudyEntry { Id = 1, Subject = "Math",    DurationInMinutes = 30 },
                new StudyEntry { Id = 2, Subject = "Math",    DurationInMinutes = 60 },
                new StudyEntry { Id = 3, Subject = "History", DurationInMinutes = 45 }
            };
            var context = CreateInMemoryDbContext();
            context.StudyEntries.AddRange(entries);
            context.SaveChanges();

            var mockLogger = new Mock<ILogger<StudyStatsService>>();
            var service    = new StudyStatsService(context, mockLogger.Object);

            // Act
            var result = await service.CalculateStatsAsync(CancellationToken.None);

            // Assert totals
            Assert.Equal(135, result.TotalMinutes);
            Assert.Equal(45, result.AverageDuration);
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
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Statistics calculated successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
