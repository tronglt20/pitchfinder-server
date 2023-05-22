using Pitch.Domain.Enums;

namespace Pitch.API.ViewModels.Store.Responses
{
    public class StoreDetailResponse
    {

        public StoreDetailResponse(Domain.Entities.Store store)
        {
            Id = store.Id;
            Name = store.Name;
            Address = store.Address;
            Status = store.Status;
            PhoneNumber = store.PhoneNumber;
            Open = store.Open;
            Close = store.Close;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public StoreStatusEnum Status { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
        public string BackgroundUrl { get; set; }
    }
}
