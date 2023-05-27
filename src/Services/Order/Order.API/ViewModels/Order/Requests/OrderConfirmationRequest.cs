namespace Order.API.ViewModels.Order.Requests
{
    public class OrderConfirmationRequest
    {
        public int StoreId { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
    }
}
