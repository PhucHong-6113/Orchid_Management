using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.PaymentService
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly string _tmnCode;
        private readonly string _hashSecret;
        private readonly string _paymentUrl;
        private readonly string _returnUrl;
        private readonly string _version;
        private readonly string _command;
        private readonly string _currCode;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
            _tmnCode = configuration["VNPay:TmnCode"];
            _hashSecret = configuration["VNPay:HashSecret"];
            _paymentUrl = configuration["VNPay:PaymentUrl"];
            _returnUrl = configuration["VNPay:ReturnUrl"];
            _version = configuration["VNPay:Version"];
            _command = configuration["VNPay:Command"];
            _currCode = configuration["VNPay:CurrCode"];
        }

        public Task<VNPaymentResponse> CreatePaymentUrl(VNPaymentRequest request)
        {
            var vnpay = new VNPayLibrary();

            vnpay.AddRequestData("vnp_Version", _version);
            vnpay.AddRequestData("vnp_Command", _command);
            vnpay.AddRequestData("vnp_TmnCode", _tmnCode);
            vnpay.AddRequestData("vnp_Amount", (request.Amount * 100).ToString(CultureInfo.InvariantCulture)); // Convert to VND

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _currCode);
            vnpay.AddRequestData("vnp_IpAddr", GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", request.Language);

            vnpay.AddRequestData("vnp_OrderInfo", request.OrderInfo);
            vnpay.AddRequestData("vnp_OrderType", request.OrderType);
            vnpay.AddRequestData("vnp_ReturnUrl", request.ReturnUrl ?? _returnUrl);
            vnpay.AddRequestData("vnp_TxnRef", request.OrderId);

            var paymentUrl = vnpay.CreateRequestUrl(_paymentUrl, _hashSecret);

            return Task.FromResult(new VNPaymentResponse
            {
                Success = true,
                PaymentUrl = paymentUrl
            });
        }

        public Task<VNPaymentReturnRequest> ProcessReturnRequest(IQueryCollection collection)
        {
            var vnpData = new Dictionary<string, string>();
            foreach (var key in collection.Keys)
            {
                var value = collection[key].FirstOrDefault();
                if (!string.IsNullOrEmpty(value))
                {
                    vnpData.Add(key, value);
                }
            }

            // Fix these methods by using TryGetValue pattern
            string orderId = null;
            vnpData.TryGetValue("vnp_TxnRef", out orderId);
            
            string vnPayTranId = null;
            vnpData.TryGetValue("vnp_TransactionNo", out vnPayTranId);
            
            string vnpResponseCode = null;
            vnpData.TryGetValue("vnp_ResponseCode", out vnpResponseCode);
            
            // Fix collection access for vnp_SecureHash
            string vnpSecureHash = null;
            if (collection.ContainsKey("vnp_SecureHash"))
            {
                vnpSecureHash = collection["vnp_SecureHash"].FirstOrDefault();
            }

            var sortedData = new SortedList<string, string>();
            foreach (var item in vnpData.Where(k => !k.Key.Equals("vnp_SecureHash", StringComparison.InvariantCultureIgnoreCase)))
            {
                sortedData.Add(item.Key, item.Value);
            }

            var isValid = ValidateSignature(sortedData, vnpSecureHash);
            
            // Fix amount conversion with null check
            decimal amount = 0;
            if (vnpData.TryGetValue("vnp_Amount", out var amountStr) && !string.IsNullOrEmpty(amountStr))
            {
                amount = Convert.ToDecimal(amountStr) / 100;
            }

            // Get other values with safe TryGetValue
            string bankCode = null;
            vnpData.TryGetValue("vnp_BankCode", out bankCode);
            
            string payDate = null;
            vnpData.TryGetValue("vnp_PayDate", out payDate);

            return Task.FromResult(new VNPaymentReturnRequest
            {
                Success = isValid && vnpResponseCode == "00",
                OrderId = orderId,
                TransactionId = vnPayTranId,
                ResponseCode = vnpResponseCode,
                BankCode = bankCode,
                PayDate = payDate,
                Amount = amount,
                RawData = vnpData
            });
        }

        public bool ValidateSignature(SortedList<string, string> vnpData, string secureHash)
        {
            var hashData = new StringBuilder();
            foreach (var (key, value) in vnpData)
            {
                hashData.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            string data = hashData.ToString().TrimEnd('&');
            string calculatedHash = HmacSHA512(_hashSecret, data);

            return calculatedHash.Equals(secureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string HmacSHA512(string key, string inputData)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using var hmac = new HMACSHA512(keyBytes);
            var hashBytes = hmac.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        private string GetIpAddress()
        {
            return "127.0.0.1"; // For local development
        }
    }
}