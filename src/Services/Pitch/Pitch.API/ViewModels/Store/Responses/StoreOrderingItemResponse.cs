using Pitch.Domain.Enums;
using System.Text.Json.Serialization;

namespace Pitch.API.ViewModels.Store.Responses
{
    public class StoreOrderingItemResponse
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
        public PitchTypeEnum PitchType { get; set; }
        public string BackgroundUrl { get; set; }

        [JsonIgnore]
        public Domain.Entities.Pitch Pitch { get; set; }
    }
}
