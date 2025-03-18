using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Categories.Category
{
    //Page to Display Specific Category Details
    public class CategoryDetailsModel : PageModel
    {
        private readonly ICategoriesService _categoryService;
        public CategoryDetailsModel(ICategoriesService categoryService)
        {
            _categoryService = categoryService;
        }
        [BindProperty]
        public Domain.Entities.Categories Category { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Category = await _categoryService.GetCategoryByIdAsync(id); //Get Category

                if (Category == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return RedirectToPage("../Error");
            }
            return Page();
        }
    }
}