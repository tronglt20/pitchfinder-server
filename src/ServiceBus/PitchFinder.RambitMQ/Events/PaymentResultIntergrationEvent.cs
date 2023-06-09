namespace PitchFinder.RambitMQ.Events
{
    public class PaymentResultIntergrationEvent : IntegrationEvent
    {
        public PaymentResultIntergrationEvent()
        {

        }

        public PaymentResultIntergrationEvent(string orderId, string message, string resultCode)
        {
            OrderId = orderId;
            Message = message;
            ResultCode = resultCode;
        }

        public string OrderId { get; set; }
        public string Message { get; set; }
        public string ResultCode { get; set; }
    }
}
