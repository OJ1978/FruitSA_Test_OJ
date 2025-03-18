using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    //Model for Categories
    public class Categories 
    {
        [Key]
        [Required]
        public int CategoryId { get; set; } //Primary Key

        [Required(ErrorMessage = "The Category Name is required.")]
        [StringLength(100, ErrorMessage = "The Category Name cannot exceed 100 characters.")]
        public string Name { get; set; } //Name with Empty and Length Validation

        [Required(ErrorMessage = "The Category Code is required.")]
        [RegularExpression(@"^[A-Z]{3}[0-9]{3}$", ErrorMessage = "Category code must be in the format ABC123 (3 letters followed by 3 numbers).")]
        [Display(Name = "Code")]
        public string CategoryCode { get; set; } //Category Code with Empty and String Format Validation, and setting Display of Field on Page

        [Display(Name = "Active?")] //Active 
        public bool IsActive { get; set; }
        public ICollection<Products>? Products { get; set; }
    }
}
