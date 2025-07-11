using System;
using System.Collections.Generic;

namespace Service.PaymentService
{
    public class VNPaymentRequest
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }
        public string ReturnUrl { get; set; }
        public string OrderType { get; set; } = "other";
        public string Language { get; set; } = "vn";
    }

    public class VNPaymentResponse
    {
        public bool Success { get; set; }
        public string PaymentUrl { get; set; }
        public string Message { get; set; }
    }

    public class VNPaymentReturnRequest
    {
        public bool Success {  set; get; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string BankCode { get; set; }
        public string PayDate { get; set; }
        public decimal Amount { get; set; }
        public Dictionary<string, string> RawData { get; set; }
    }
}