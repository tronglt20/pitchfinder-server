namespace Order.API.ViewModels.Order.Responses
{
    public class OrderConfirmationResponse
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int PitchId { get; set; }
        public string PitchName { get; set; }
        public string Address { get; set; }
        public int PitchType { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
    }
}
