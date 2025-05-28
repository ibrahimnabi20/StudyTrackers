namespace StudyTracker.Models
{
    public class FeatureToggles
    {
        public bool EnableCreateEntry { get; set; } = true;
        public bool EnableStudyStats { get; set; } = true;
        public bool EnableAdvancedExport { get; set; } = false;
    }
}
