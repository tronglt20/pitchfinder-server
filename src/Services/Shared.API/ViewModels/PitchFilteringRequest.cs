namespace Shared.API.ViewModels
{
    public class PitchFilteringRequest
    {
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
    }
}
