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
        public async Task CalculateStatsAsync_ComputesCorrectValues()
        {
            var entries = new List<StudyEntry>
            {
                new StudyEntry { Id = 1, Subject = "A", DurationInMinutes = 30 },
                new StudyEntry { Id = 2, Subject = "A", DurationInMinutes = 60 },
                new StudyEntry { Id = 3, Subject = "B", DurationInMinutes = 45 }
            };
            var service = CreateServiceWithEntries(entries);

            var result = await service.CalculateStatsAsync(CancellationToken.None);

            // Total = 30+60+45 = 135
            Assert.Equal(135, result.TotalMinutes);
            // Average = 135/3 = 45
            Assert.Equal(45, result.AverageDuration);

            // PerSubject: A => (30+60)/2 = 45, B => 45/1 = 45
            Assert.Collection(result.PerSubject,
                a =>
                {
                    Assert.Equal("A", a.Subject);
                    Assert.Equal(2, a.SessionCount);
                    Assert.Equal(45, a.AverageDuration);
                },
                b =>
                {
                    Assert.Equal("B", b.Subject);
                    Assert.Equal(1, b.SessionCount);
                    Assert.Equal(45, b.AverageDuration);
                });
        }
    }
}
