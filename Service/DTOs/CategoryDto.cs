using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Service.DTOs
{
    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        
        [JsonPropertyName("category_name")]
        public string Name { get; set; }
        
        [JsonPropertyName("orchid_count")]
        public int OrchidCount { get; set; }
    }

    public class CreateCategoryDto 
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; }
    }
}