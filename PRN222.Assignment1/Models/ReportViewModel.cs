
namespace PRN222.Assignment1.Models
{
    public class ReportViewModel
    {
        public int TotalEvents { get; set; }
        public int TotalAttendees { get; set; }
        public Dictionary<string, int> TotalAttendeesByCategory { get; set; } = new Dictionary<string, int>();
        public List<EventTrend> EventTrends { get; set; } = new List<EventTrend>();
        public Dictionary<string, int> TotalEventsByCategory { get; set; } = new Dictionary<string, int>();
    }
}
