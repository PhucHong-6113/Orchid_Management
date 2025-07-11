using Service.DTOs;
using Service.PaymentService;

namespace Service.OrderService
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(Guid customerId, CreateOrderDto orderDto);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<bool> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDto statusDto);

        Task<VNPaymentResponse> CreatePaymentForOrderAsync(Guid orderId, string returnUrl = null);
        Task<bool> ProcessPaymentReturnAsync(VNPaymentReturnRequest returnRequest);
    }
}