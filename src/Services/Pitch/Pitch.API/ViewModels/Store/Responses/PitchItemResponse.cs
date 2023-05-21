using Pitch.Domain.Enums;

namespace Pitch.API.ViewModels.Store.Responses
{
    public class PitchItemResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
        public PitchStatusEnum Status { get; set; }
    }
}
