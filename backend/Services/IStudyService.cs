using StudyTracker.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public interface IStudyService
    {
        Task<List<StudyEntry>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<StudyEntry?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<StudyEntry> CreateAsync(StudyEntry entry, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(StudyEntry entry, CancellationToken cancellationToken = default);
    }
}
