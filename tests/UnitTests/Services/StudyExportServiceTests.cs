using System;
using System.Collections.Generic;
using System.Text;
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
        private StudyDbContext CreateInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<StudyDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new StudyDbContext(options);
        }

        [Fact]
        public void ExportToCsv_ReturnsEmptyByteArray_WhenNoEntries()
        {
            // Arrange
            var context = CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var logger  = Mock.Of<ILogger<StudyExportService>>();
            var service = new StudyExportService(context, logger);

            // Act
            var result = service.ExportToCsv();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ExportToCsv_ReturnsCorrectCsv_WhenEntriesExist()
        {
            // Arrange
            var context = CreateInMemoryDbContext(Guid.NewGuid().ToString());
            context.StudyEntries.AddRange(new[]
            {
                new StudyEntry { Id = 1, Subject = "Math", DurationInMinutes = 60, Timestamp = new DateTime(2024, 1, 1) },
                new StudyEntry { Id = 2, Subject = "Science", DurationInMinutes = 45, Timestamp = new DateTime(2024, 1, 2) }
            });
            context.SaveChanges();

            var logger  = Mock.Of<ILogger<StudyExportService>>();
            var service = new StudyExportService(context, logger);

            // Act
            var bytes = service.ExportToCsv();
            var csv   = Encoding.UTF8.GetString(bytes);

            // Assert
            Assert.Contains("Id,Subject,DurationInMinutes,Timestamp", csv);
            Assert.Contains("1,\"Math\",60,2024-01-01T00:00:00", csv);
            Assert.Contains("2,\"Science\",45,2024-01-02T00:00:00", csv);
        }
    }
}
