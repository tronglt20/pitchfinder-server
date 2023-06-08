namespace PitchFinder.RambitMQ.Events
{
    public class OrderCreatedIntergrationEvent : IntegrationEvent
    {
        public OrderCreatedIntergrationEvent()
        {

        }

        public OrderCreatedIntergrationEvent(int orderId, int amount)
        {
            OrderId = orderId;
            Amount = amount;
        }

        public int OrderId { get; set; }
        public int Amount { get; set; }
    }
}
