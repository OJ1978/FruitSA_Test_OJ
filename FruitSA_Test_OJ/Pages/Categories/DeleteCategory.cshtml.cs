using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Categories
{
    //Page to Handle Deletion of Category
    public class DeleteCategoryModel : PageModel
    {
        private readonly ICategoriesService _categoryService;
        public DeleteCategoryModel(ICategoriesService categoryService)
        {
            _categoryService = categoryService; //Get Service
        }
        [BindProperty]
        public Domain.Entities.Categories Category { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _categoryService.GetCategoryByIdAsync(id); //Get Category

            if (Category == null)
            {
                return NotFound(); //If Not Found
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id); //Get Category

            if (category != null)
            {
                await _categoryService.DeleteCategoryAsync(id); //Delete Category
            }

            return RedirectToPage("./Category");
        }
    }
}