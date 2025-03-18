using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Products.Product
{
    //Page to Display Specific Product Details
    public class ProductDetailsModel : PageModel
    {
        private readonly IProductsService _productService;

        public ProductDetailsModel(IProductsService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Domain.Entities.Products Product { get; set; }
        public SelectList CategoryList { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Product = await _productService.GetProductByIdAsync(id); //Get Product

                if (Product == null)
                {
                    return NotFound();
                }
                CategoryList = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "Name"); //Populate Select List
            }
            catch (Exception ex)
            {
                return RedirectToPage("../Error");
            }
            return Page();
        }

        public async Task<IActionResult> OnGetProductImageAsync(int id)
        {
            var imageData = await _productService.GetProductImageByIdAsync(id); //Get Product Image

            if (imageData == null || imageData.Length == 0)
            {
                return NotFound();
            }

            var base64Image = Convert.ToBase64String(imageData);

            return new JsonResult(new { base64Image });
        }

    }
}
