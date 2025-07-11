using BusinessObject;
using Repository.Orchid;
using Repository.Order;
using Service.DTOs;
using Service.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrchidRepository _orchidRepository;
        private readonly IVNPayService _vnPayService;

        public OrderService(
            IOrderRepository orderRepository,
            IOrchidRepository orchidRepository,
            IVNPayService vnPayService)
        {
            _orderRepository = orderRepository;
            _orchidRepository = orchidRepository;
            _vnPayService = vnPayService;
        }

        public OrderService(IOrderRepository orderRepository, IOrchidRepository orchidRepository)
        {
            _orderRepository = orderRepository;
            _orchidRepository = orchidRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Select(MapOrderToDto);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
                return null;

            return MapOrderToDto(order);
        }

        public async Task<OrderDto> CreateOrderAsync(Guid customerId, CreateOrderDto orderDto)
        {
            decimal totalAmount = 0;
            var orderDetails = new List<OrderDetail>();

            foreach (var item in orderDto.OrderItems)
            {
                var orchid = await _orchidRepository.GetOrchidByIdAsync(item.OrchidId);
                if (orchid == null)
                    throw new Exception($"Orchid with ID {item.OrchidId} not found");

                var detail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrchidId = orchid.OrchidId,
                    Quantity = item.Quantity,
                    Price = orchid.Price
                };

                totalAmount += orchid.Price * item.Quantity;
                orderDetails.Add(detail);
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending",
                TotalAmount = totalAmount,
                AccountId = customerId
            };

            var createdOrder = await _orderRepository.CreateOrderAsync(order, orderDetails);
            return MapOrderToDto(createdOrder);
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDto statusDto)
        {
            return await _orderRepository.UpdateOrderStatusAsync(orderId, statusDto.Status);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return orders.Select(MapOrderToDto);
        }

        private OrderDto MapOrderToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Status = order.OrderStatus,
                TotalAmount = order.TotalAmount,
                CustomerId = order.AccountId,
                CustomerName = order.Account?.AcountName ?? "Unknown",
                OrderItems = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrchidId = od.OrchidId,
                    OrchidName = od.Orchid?.OrchidName ?? "Unknown",
                    UnitPrice = od.Price,
                    Quantity = od.Quantity,
                    ImageUrl = od.Orchid?.OrchidUrl
                }).ToList()
            };
        }

        public async Task<VNPaymentResponse> CreatePaymentForOrderAsync(Guid orderId, string returnUrl = null)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            if (order.OrderStatus != "Pending")
                throw new Exception("Order is not in pending state");

            var vnPayRequest = new VNPaymentRequest
            {
                OrderId = order.Id.ToString(),
                Amount = order.TotalAmount,
                OrderInfo = $"Thanh toan don hang: {order.Id}",
                OrderType = "orchid",
                ReturnUrl = returnUrl
            };

            await _orderRepository.UpdateOrderStatusAsync(orderId, "Processing");

            return await _vnPayService.CreatePaymentUrl(vnPayRequest);
        }

        public async Task<bool> ProcessPaymentReturnAsync(VNPaymentReturnRequest returnRequest)
        {
            if (!Guid.TryParse(returnRequest.OrderId, out Guid orderId))
                return false;

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return false;

            string newStatus = returnRequest.ResponseCode == "00" ? "Complete" : "Failed";

            return await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
        }
    }
}
