using Pitch.Domain.Enums;
using Shared.API.ValidataionAttributes;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class AddPitchRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public PitchTypeEnum Type { get; set; }

        [FileSizeAttribute(20 * 1024 * 1024)] // max file size is 20MB
        public List<IFormFile> Formfiles { get; set; } = new List<IFormFile>();
    }
}
