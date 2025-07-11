using Microsoft.AspNetCore.Http;

namespace Service.PaymentService
{
    public interface IVNPayService
    {
        Task<VNPaymentResponse> CreatePaymentUrl(VNPaymentRequest request);
        Task<VNPaymentReturnRequest> ProcessReturnRequest(IQueryCollection collection);
        bool ValidateSignature(SortedList<string, string> vnpData, string secureHash);
    }
}