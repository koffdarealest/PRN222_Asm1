using PRN222.Assignment2.Models.Validations;

namespace PRN222.Assignment2.Models
{
    public class EventCreateViewModel
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [StartTimeBeforeNow]
        public DateTime? StartTime { get; set; }

        [EndTimeAfterStartTime]
        public DateTime? EndTime { get; set; }

        public string? Location { get; set; }

        public int CategoryId { get; set; }
    }
}
