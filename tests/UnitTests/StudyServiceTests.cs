using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StudyTracker.Data;
using StudyTracker.Models;
using StudyTracker.Services;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class StudyServiceTests
    {
        private StudyDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<StudyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new StudyDbContext(options);
        }

        private IOptions<FeatureToggles> CreateFeatureToggles(bool enabled = true)
        {
            return Options.Create(new FeatureToggles { EnableCreateEntry = enabled });
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntries_AndLog()
        {
            var context = CreateInMemoryDbContext();
            context.StudyEntries.Add(new StudyEntry { Subject = "Test1", DurationInMinutes = 30 });
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles());

            var result = await service.GetAllAsync(CancellationToken.None);

            Assert.Single(result);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching all study entries from the database")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectEntry_AndLog()
        {
            var context = CreateInMemoryDbContext();
            context.StudyEntries.Add(new StudyEntry { Id = 1, Subject = "Test2", DurationInMinutes = 45 });
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles());

            var result = await service.GetByIdAsync(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Test2", result.Subject);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching study entry with ID")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntry_AndLog()
        {
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles());

            var entry = new StudyEntry { Subject = "TestCreate", DurationInMinutes = 25 };
            var result = await service.CreateAsync(entry, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(context.StudyEntries);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Creating a new study entry")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenFeatureToggleDisabled()
        {
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles(false));
            var entry = new StudyEntry { Subject = "ShouldFail" };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(entry, CancellationToken.None));
            Assert.Equal("Creating entries is currently disabled.", ex.Message);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Attempt to create entry while feature toggle is disabled")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntry_AndLog()
        {
            var context = CreateInMemoryDbContext();
            context.StudyEntries.Add(new StudyEntry { Id = 1, Subject = "DeleteTest" });
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles());

            var result = await service.DeleteAsync(1, CancellationToken.None);

            Assert.True(result);
            Assert.Empty(context.StudyEntries);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Deleting study entry with ID")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateFields_AndLog()
        {
            var context = CreateInMemoryDbContext();
            context.StudyEntries.Add(new StudyEntry { Id = 1, Subject = "Old", DurationInMinutes = 10, Timestamp = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles());

            var updatedEntry = new StudyEntry { Id = 1, Subject = "New", DurationInMinutes = 20, Timestamp = DateTime.UtcNow.AddMinutes(10) };
            var result = await service.UpdateAsync(updatedEntry, CancellationToken.None);

            Assert.True(result);
            var entry = await context.StudyEntries.FindAsync(1);
            Assert.Equal("New", entry.Subject);
            Assert.Equal(20, entry.DurationInMinutes);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Updating study entry with ID")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_AndLogWarning_IfNotFound()
        {
            var context = CreateInMemoryDbContext();
            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles());

            var updatedEntry = new StudyEntry { Id = 999, Subject = "NoMatch" };
            var result = await service.UpdateAsync(updatedEntry, CancellationToken.None);

            Assert.False(result);
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Attempted to update non-existent entry with ID")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldActuallyPersistChangeToDatabase()
        {
            var context = CreateInMemoryDbContext();
            var entry = new StudyEntry
            {
                Id = 1,
                Subject = "Original",
                DurationInMinutes = 20,
                Timestamp = DateTime.UtcNow
            };

            context.StudyEntries.Add(entry);
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<StudyService>>();
            var service = new StudyService(context, mockLogger.Object, CreateFeatureToggles());

            var updated = new StudyEntry
            {
                Id = 1,
                Subject = "Changed",
                DurationInMinutes = 99,
                Timestamp = DateTime.UtcNow.AddMinutes(5)
            };

            var result = await service.UpdateAsync(updated, CancellationToken.None);

            Assert.True(result);

            var reloaded = await context.StudyEntries.FindAsync(1);
            Assert.Equal("Changed", reloaded.Subject);
            Assert.Equal(99, reloaded.DurationInMinutes);

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Updating study entry with ID")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
