using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Service.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderDetailDto> OrderItems { get; set; } = new List<OrderDetailDto>();
    }

    public class OrderDetailDto
    {
        public Guid OrchidId { get; set; }
        public string OrchidName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        [Required]
        public Guid OrchidId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        [Required]
        public string Status { get; set; }
    }
}