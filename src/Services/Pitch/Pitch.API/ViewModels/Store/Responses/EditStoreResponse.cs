namespace Pitch.API.ViewModels.Store.Responses
{
    public class EditStoreResponse
    {
        public EditStoreResponse(Domain.Entities.Store store)
        {
            Id = store.Id;
            Name = store.Name;
            Address = store.Address;
            PhoneNumber = store.PhoneNumber;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
