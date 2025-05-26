using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Services;
using Xunit;

namespace UnitTests
{
    public class StudyServiceTests
    {
        // Creates a new in-memory EF Core context per test using a unique database name
        private StudyService GetInMemoryService()
        {
            var options = new DbContextOptionsBuilder<StudyDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // Unique DB per test run
                .Options;

            var context = new StudyDbContext(options);
            return new StudyService(context);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntry()
        {
            // Arrange
            var service = GetInMemoryService();
            var entry = new StudyEntry { Subject = "Math", DurationInMinutes = 60 };

            // Act
            var result = await service.CreateAsync(entry);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Math", result.Subject);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEntries()
        {
            // Arrange
            var service = GetInMemoryService();
            await service.CreateAsync(new StudyEntry { Subject = "Science", DurationInMinutes = 45 });

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Single(result);
        }
    }
}
