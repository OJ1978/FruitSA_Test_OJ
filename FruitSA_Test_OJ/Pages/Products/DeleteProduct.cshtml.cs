using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Products.Product
{
    //Page to Handle Deletion of Product
    public class DeleteProductModel : PageModel
    {
        private readonly IProductsService _productService;

        public DeleteProductModel(IProductsService productService)
        {
            _productService = productService; //Get Service
        }

        [BindProperty]
        public Domain.Entities.Products Product { get; set; }
        public SelectList CategoryList { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id); //Get Product

            if (Product == null)
            {
                return NotFound(); //If Not Found
            }
            CategoryList = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "Name"); //Populate Category List

            return Page();
        }

        public async Task<IActionResult> OnGetProductImageAsync(int id)
        {
            var imageData = await _productService.GetProductImageByIdAsync(id); //Get Product Image

            if (imageData == null || imageData.Length == 0)
            {
                return NotFound(); //Page to Handle Editing of Product
            }

            var base64Image = Convert.ToBase64String(imageData); //Get Image Data
            return new JsonResult(new { base64Image });
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id); //Get Product

            if (product != null)
            {
                await _productService.DeleteProductAsync(id); //Delete Product
            }

            return RedirectToPage("./Product");
        }
    }
}
