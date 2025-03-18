using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Products
    {
        [Key]
        [Required]
        public int ProductId { get; set; } //Primary Key

        [Required(ErrorMessage = "The Product Code is required.")]
        [StringLength(10, ErrorMessage = "The Product Code cannot exceed 10 characters.")]
        [RegularExpression(@"^\d{6}-\d{3}$", ErrorMessage = "Product Code must be in the format yyyyMM-###")]
        [Display(Name = "Code")]
        public string ProductCode { get; set; } //Product Code with Empty, Length, and Format Validation and Display value for Page

        [Required(ErrorMessage = "The Product Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters long")]
        public string Name { get; set; } //Name with Empty and Length Validation
        public string? Description { get; set; } //Description

        [Required(ErrorMessage = "The Category Name is required.")]
        [StringLength(100, ErrorMessage = "The Category Name cannot exceed 100 characters.")]
        [Display(Name = "Category")] //Category Name with Empty and Length Validation and Display value for Page
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; } //Price with Empty and Number Format Validation
        public byte[]? ImageUrl { get; set; } //Image

        public DateTime CreatedDate { get; set; } //Created Date 
        public string? CreatedBy { get; set; } //Created By
        [Display(Name = "Category")]
        public int CategoryId { get; set; } //Category ID Linked to Product with Display value for Page
        public Categories? Category { get; set; }

    }
}
