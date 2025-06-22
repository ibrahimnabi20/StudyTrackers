using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StudyTracker.Controllers;
using StudyTracker.Models;
using StudyTracker.Services;
using Xunit;

namespace UnitTests.Controllers
{
    public class ExportControllerTests
    {
        private readonly Mock<IStudyExportService> _mockExportService;
        private readonly Mock<IOptions<FeatureToggles>> _mockToggles;
        private readonly Mock<ILogger<ExportController>> _mockLogger;
        private readonly ExportController _controller;

        public ExportControllerTests()
        {
            _mockExportService = new Mock<IStudyExportService>();
            _mockToggles       = new Mock<IOptions<FeatureToggles>>();
            _mockLogger        = new Mock<ILogger<ExportController>>();

            // Always enable advanced export by default in these tests
            _mockToggles.Setup(t => t.Value)
                        .Returns(new FeatureToggles { EnableAdvancedExport = true });

            _controller = new ExportController(
                _mockExportService.Object,
                _mockToggles.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public void GetExport_ReturnsNoContent_WhenNoData()
        {
            // Arrange: service returns empty byte array
            _mockExportService.Setup(s => s.ExportToCsv())
                              .Returns(Array.Empty<byte>());

            // Act
            var result = _controller.GetExport();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetExport_ReturnsFile_WhenDataExists()
        {
            // Arrange: service returns some CSV bytes
            var csvData = new byte[] { 1, 2, 3 };
            _mockExportService.Setup(s => s.ExportToCsv())
                              .Returns(csvData);

            // Act
            var actionResult = _controller.GetExport();
            var fileResult   = Assert.IsType<FileContentResult>(actionResult);

            // Assert file metadata
            Assert.Equal("text/csv", fileResult.ContentType);
            Assert.Equal(csvData, fileResult.FileContents);
            Assert.StartsWith("study-entries_", fileResult.FileDownloadName);
            Assert.EndsWith(".csv", fileResult.FileDownloadName);
        }
    }
}
