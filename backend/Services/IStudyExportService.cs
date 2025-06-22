// Services/IStudyExportService.cs
namespace StudyTracker.Services
{
    public interface IStudyExportService
    {
        /// <summary>
        /// Exports all study entries to CSV, as a UTF-8 byte array.
        /// Returns an empty array if no entries exist.
        /// </summary>
        byte[] ExportToCsv();
    }
}