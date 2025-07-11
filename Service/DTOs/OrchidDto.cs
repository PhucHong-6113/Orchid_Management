using System.ComponentModel.DataAnnotations;

namespace Service.DTOs
{
    public class OrchidDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsNatural { get; set; }
    }

    public class CreateOrchidDto
    {
        [Required(ErrorMessage = "Orchid name is required")]
        public string OrchidName { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 250 characters")]
        public string OrchidDescription { get; set; } = null!;

        // Nullable, can be blank
        public string? OrchidUrl { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        public bool IsNatural { get; set; }
    }
}
