using MassTransit;
using Newtonsoft.Json.Linq;
using PitchFinder.RambitMQ.Events;

namespace Payment.Momo.Services
{
    public class MomoService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MomoService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> PaymentAsync(string orderId, string amount)
        {
            //request params need to request to MoMo system
            string endpoint = MomoCredential.Endpoint;
            string partnerCode = MomoCredential.PartnerCode;
            string accessKey = MomoCredential.AccessKey;
            string serectkey = MomoCredential.Serectkey;
            string redirectUrl = MomoCredential.RedirectUrl;
            string ipnUrl = MomoCredential.IpnUrl;
            string requestType = MomoCredential.RequestType;
            string orderInfo = "PitchFinder App";

            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            return jmessage.ToString();
        }

        public async Task ReceivePaymentResultAsync(MomoPaymentResult paymentResult)
        {
            var orderId = paymentResult.OrderId.Split("--")[1];

            await _publishEndpoint.Publish(new PaymentResultIntergrationEvent(orderId
                , paymentResult.Message
                , paymentResult.ResultCode));
        }
    }
}
