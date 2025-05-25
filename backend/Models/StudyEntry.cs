namespace StudyTracker.Models
{
    public class StudyEntry
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        


    }
}