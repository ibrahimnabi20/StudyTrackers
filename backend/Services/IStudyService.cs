using StudyTracker.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public interface IStudyService
    {
        Task<List<StudyEntry>> GetAllAsync(CancellationToken cancellationToken);
        Task<StudyEntry?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<StudyEntry> CreateAsync(StudyEntry entry, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(StudyEntry updatedEntry, CancellationToken cancellationToken);
    }
}
