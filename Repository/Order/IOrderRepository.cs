using BusinessObject;

namespace Repository.Order
{
    public interface IOrderRepository
    {
        Task<BusinessObject.Order> CreateOrderAsync(BusinessObject.Order order, List<OrderDetail> orderDetails);
        Task<IEnumerable<BusinessObject.Order>> GetAllOrdersAsync();
        Task<BusinessObject.Order> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<BusinessObject.Order>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<bool> UpdateOrderStatusAsync(Guid orderId, string status);
    }
}