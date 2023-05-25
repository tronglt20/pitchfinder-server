namespace Shared.API.ViewModels
{
    public class PitchFilteringRequest
    {
        public DateTime Date { get; set; }
        public int PitchType { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
