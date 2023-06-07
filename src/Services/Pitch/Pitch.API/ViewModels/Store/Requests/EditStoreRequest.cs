using Shared.API.ValidataionAttributes;

namespace Pitch.API.ViewModels.Store.Requests
{
    public class EditStoreRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }

        [FileSizeAttribute(20 * 1024 * 1024)] // max file size is 20MB
        public IFormFile? BackgroundImage { get; set; }
    }
}
