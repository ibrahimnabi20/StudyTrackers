using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Services;
using Xunit;

namespace UnitTests.Services
{
    public class StudyExportServiceTests
    {
        private StudyDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<StudyDbContext>()
                .UseInMemoryDatabase("TestDb_" + Guid.NewGuid())
                .Options;
            return new StudyDbContext(options);
        }

        private StudyExportService CreateServiceWithEntries(List<StudyEntry> entries)
        {
            var context = CreateInMemoryDbContext();
            context.StudyEntries.AddRange(entries);
            context.SaveChanges();

            var logger = new Mock<ILogger<StudyExportService>>();
            return new StudyExportService(context, logger.Object);
        }

        [Fact]
        public void ExportToCsv_ReturnsEmptyString_WhenNoEntriesExist()
        {
            var service = CreateServiceWithEntries(new List<StudyEntry>());

            // ExportToCsv() returnerer nu en string
            var csv = service.ExportToCsv();

            Assert.Equal(string.Empty, csv);
        }

        [Fact]
        public void ExportToCsv_ReturnsCorrectCsv_WhenEntriesExist()
        {
            var entries = new List<StudyEntry>
            {
                new StudyEntry { Id = 1, Subject = "Math",    DurationInMinutes = 60, Date = new DateTime(2024,1,1) },
                new StudyEntry { Id = 2, Subject = "Science", DurationInMinutes = 45, Date = new DateTime(2024,1,2) }
            };

            var service = CreateServiceWithEntries(entries);

            // Nu returneres en string, så vi behøver ikke byte[] ↔️ string-konvertering
            var csv = service.ExportToCsv();

            // Matcher header
            Assert.Contains("Id,Subject,DurationInMinutes,Date", csv);
            // Matcher data-linjer (med anførselstegn omkring Subject)
            Assert.Contains("1,\"Math\",60,2024-01-01", csv);
            Assert.Contains("2,\"Science\",45,2024-01-02", csv);
        }
    }
}
