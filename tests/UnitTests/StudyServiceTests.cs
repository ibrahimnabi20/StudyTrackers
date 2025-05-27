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
        private StudyDbContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<StudyDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new StudyDbContext(options);
        }

        [Fact]
        public async Task CreateAndGetAll_ShouldReturnCreatedEntry()
        {
            // Arrange
            var context = GetTestDbContext();
            var service = new StudyService(context);
            var newEntry = new StudyEntry { Subject = "Math", DurationInMinutes = 45 };

            // Act
            await service.CreateAsync(newEntry);
            var all = await service.GetAllAsync();

            // Assert
            Assert.Single(all);
            Assert.Equal("Math", all[0].Subject);
        }

        [Fact]
        public async Task DeleteAndGetById_ShouldReturnNullAfterDeletion()
        {
            // Arrange
            var context = GetTestDbContext();
            var service = new StudyService(context);
            var created = await service.CreateAsync(new StudyEntry { Subject = "Physics", DurationInMinutes = 30 });

            // Act
            var fetched = await service.GetByIdAsync(created.Id);
            await service.DeleteAsync(created.Id);
            var afterDelete = await service.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal("Physics", fetched.Subject);
            Assert.Null(afterDelete);
        }
    }
}
