using System;

namespace StudyTracker.Models
{
    public class StudyEntry
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty; 
        public int DurationInMinutes { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
