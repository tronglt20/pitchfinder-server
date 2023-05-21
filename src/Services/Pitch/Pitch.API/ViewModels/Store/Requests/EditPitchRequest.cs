using Pitch.Domain.Enums;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class EditPitchRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public PitchStatusEnum Status { get; set; } = PitchStatusEnum.Open;
    }
}
