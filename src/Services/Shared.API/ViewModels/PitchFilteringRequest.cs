namespace Shared.API.ViewModels
{
    public class PitchFilteringRequest
    {
        public DateTime Date { get; set; }
        public int PitchType { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
    }
}
