using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Categories.Category
{
    //Page to Handle Creation of Category
    public class CreateCategoryModel : PageModel
    {
        private readonly ICategoriesService _categoryService;
        public CreateCategoryModel(ICategoriesService categoryService)
        {
            _categoryService = categoryService; //Get Service
        }

        [BindProperty]
        public Domain.Entities.Categories Category { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _categoryService.AddCategoryAsync(Category); //Add Category

            return RedirectToPage("./Category");
        }
    }
}
