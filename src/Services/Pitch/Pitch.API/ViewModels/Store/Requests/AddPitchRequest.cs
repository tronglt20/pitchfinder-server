using Pitch.Domain.Enums;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class AddPitchRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public PitchTypeEnum Type { get; set; }
    }
}
