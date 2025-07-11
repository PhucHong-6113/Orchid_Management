using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Service.DTOs
{
    public class OrderDto
    {
        [JsonPropertyName("order_id")]
        public Guid OrderId { get; set; }
        
        [JsonPropertyName("order_date")]
        public DateTime OrderDate { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }
        
        [JsonPropertyName("customer_id")]
        public Guid CustomerId { get; set; }
        
        [JsonPropertyName("customer_name")]
        public string CustomerName { get; set; }
        
        [JsonPropertyName("order_items")]
        public List<OrderDetailDto> OrderItems { get; set; } = new List<OrderDetailDto>();
    }

    public class OrderDetailDto
    {
        [JsonPropertyName("orchid_id")]
        public Guid OrchidId { get; set; }
        
        [JsonPropertyName("orchid_name")]
        public string OrchidName { get; set; }
        
        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }
        
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        [JsonPropertyName("order_items")]
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        [Required]
        [JsonPropertyName("orchid_id")]
        public Guid OrchidId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        [Required]
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}