using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Categories.Category
{
    //Page to Handle Editing of Category
    public class EditCategoryModel : PageModel
    {
        private readonly ICategoriesService _categoryService;

        public EditCategoryModel(ICategoriesService categoryService)
        {
            _categoryService = categoryService; //Get Service
        }

        [BindProperty]
        public Domain.Entities.Categories Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _categoryService.GetCategoryByIdAsync(id); //Get Category to Edit

            if (Category == null)
            {
                return NotFound(); //If Not Found
            }

            return Page();
        }

        public async Task<IActionResult> OnGetIsActiveByIDAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return new JsonResult(new { IsActive = category.IsActive });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) //If Not All Required Fields Populated
            {
                return Page();
            }

            var categoryId = Category.CategoryId;

            var category = await _categoryService.GetCategoryByIdAsync(categoryId); //Get Current Category

            if (category == null)
            {
                return NotFound();
            }

            //Update Category 
            category.Name = Category.Name;
            category.CategoryCode = Category.CategoryCode;
            category.IsActive = Category.IsActive;

            //Update Products with this Category
            var products = await _categoryService.GetProductsByCategoryIdAsync(categoryId);

            foreach (var product in products)
            {
                product.CategoryName = Category.Name; //Update CategoryName 
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(category); //Update Categories Table
                await _categoryService.UpdateProductsAsync(products); //Update Products Table
            }
            catch (Exception)
            {
                var currentCategory = await _categoryService.GetCategoryByIdAsync(categoryId); //Reload Current Category

                if (currentCategory == null) //If Not Found
                {
                    return NotFound();
                }

                Category = currentCategory; //Reload Category with Current Values
                return Page(); //Reload form
            }

            return RedirectToPage("./Category");
        }
    }
}
