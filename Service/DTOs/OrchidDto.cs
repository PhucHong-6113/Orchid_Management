using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Service.DTOs
{
    public class OrchidDto
    {
        [JsonPropertyName("orchid_id")]
        public Guid Id { get; set; }

        [JsonPropertyName("orchid_name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("category")]
        public OrchidCategoryDto Category { get; set; }

        [JsonPropertyName("orchid_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("is_natural")]
        public bool IsNatural { get; set; }
    }

    public class OrchidCategoryDto
    {
        [JsonPropertyName("category_id")]
        public Guid CategoryId { get; set; }

        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; }
    }

    public class CreateOrchidDto
    {
        [Required(ErrorMessage = "Orchid name is required")]
        [JsonPropertyName("orchid_name")]
        public string OrchidName { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 250 characters")]
        [JsonPropertyName("orchid_description")]
        public string OrchidDescription { get; set; } = null!;

        [JsonPropertyName("orchid_url")]
        public string? OrchidUrl { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
         [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [JsonPropertyName("category_id")]
        public Guid CategoryId { get; set; }

        [JsonPropertyName("is_natural")]
        public bool IsNatural { get; set; }
    }

    public class OrchidSearchDto
    {
        [JsonPropertyName("orchid_name")]
        public string? Name { get; set; }

        [JsonPropertyName("category_id")]
        public Guid? CategoryId { get; set; }

        [JsonPropertyName("is_natural")]
        public bool? IsNatural { get; set; }

        [JsonPropertyName("min_price")]
        public decimal? MinPrice { get; set; }

        [JsonPropertyName("max_price")]
        public decimal? MaxPrice { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 10;
    }
}
