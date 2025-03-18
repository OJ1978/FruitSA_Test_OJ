using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence.Services;


namespace FruitSA_Test_OJ.Pages.Products.Product
{
    //Page to Handle Creation of Product
    public class CreateProductModel : PageModel
    {
        private readonly IProductsService _productService;

        public CreateProductModel(IProductsService productService)
        {
            _productService = productService; //Get Service
        }

        [BindProperty]
        public Domain.Entities.Products Product { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }
        public SelectList CategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CategoryList = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "Name"); //Get Categories
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Product.CreatedDate = DateTime.Now;
            Product.ProductCode = await _productService.GenerateProductCodeAsync(); //Generate Product Code
            Product.CategoryName = await _productService.GetCategoryNameByIdAsync(Product.CategoryId); //Get Selected Category

            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(memoryStream); //Add Image
                    Product.ImageUrl = memoryStream.ToArray();
                }
            }

            await _productService.AddProductAsync(Product); //Add Product
            return RedirectToPage("./Product");
        }
    }
}
