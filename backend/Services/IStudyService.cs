using StudyTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public interface IStudyService
    {
        Task<List<StudyEntry>> GetAllAsync();
        Task<StudyEntry> GetByIdAsync(int id);
        Task<StudyEntry> CreateAsync(StudyEntry entry);
        Task<bool> DeleteAsync(int id);
    }
}