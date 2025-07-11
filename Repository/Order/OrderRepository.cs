using BusinessObject;
using DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Order
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IGenericRepository<BusinessObject.Order> _orderRepository;
        private readonly IGenericRepository<BusinessObject.OrderDetail> _orderDetailRepository;
        private readonly OrchidManagamentContext _context;

        public OrderRepository(
            IGenericRepository<BusinessObject.Order> orderRepository,
            IGenericRepository<BusinessObject.OrderDetail> orderDetailRepository,
            OrchidManagamentContext context)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _context = context;
        }

        public async Task<IEnumerable<BusinessObject.Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Orchid)
                .ToListAsync();
        }

        public async Task<BusinessObject.Order> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Orchid)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<BusinessObject.Order> CreateOrderAsync(BusinessObject.Order order, List<BusinessObject.OrderDetail> orderDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _orderRepository.Add(order);
                await _orderRepository.SaveChangesAsync();

                foreach (var detail in orderDetails)
                {
                    detail.Id = order.Id;
                    _orderDetailRepository.Add(detail);
                }
                await _orderDetailRepository.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetOrderByIdAsync(order.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid Id, string status)
        {
            var order = await _context.Orders.FindAsync(Id);
            if (order == null)
                return false;

            order.OrderStatus = status;
            _context.Orders.Update(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<BusinessObject.Order>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            return await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Orchid)
                .Where(o => o.AccountId == customerId)
                .ToListAsync();
        }
    }
}