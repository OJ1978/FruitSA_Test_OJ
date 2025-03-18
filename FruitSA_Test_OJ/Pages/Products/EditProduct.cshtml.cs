using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Products.Product
{
    //Page to Handle Editing of Product
    public class EditProductModel : PageModel
    {
        private readonly IProductsService _productService;
        private readonly ICategoriesService _categoryService;

        public EditProductModel(IProductsService productService, ICategoriesService categoryService)
        {
            _productService = productService; //Get Product Service
            _categoryService = categoryService; //Get Category Service
        }

        [BindProperty]
        public Domain.Entities.Products Product { get; set; }
        public SelectList CategoryList { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id); //Get Product to Edit

            if (Product == null)
            {
                return NotFound();//If Not Found
            }

            CategoryList = new SelectList(await _productService.GetCategoriesAsync(), "CategoryId", "Name"); //Get Category Associated with Product

            return Page();
        }

        public async Task<IActionResult> OnGetGetProductImageAsync(int id)
        {
            var imageData = await _productService.GetProductImageByIdAsync(id); //Get Product Image

            if (imageData == null || imageData.Length == 0)
            {
                return NotFound(); //If Not Found
            }

            var base64Image = Convert.ToBase64String(imageData); //Get Image Contents
            return new JsonResult(new { base64Image }); 
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) //If Not All Required Fields Populated
            {
                return Page();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(memoryStream); //Get Image
                    Product.ImageUrl = memoryStream.ToArray();
                }
            }
            else
            {
                Product.ImageUrl = Product.ImageUrl;
            }

            var productId = Product.ProductId;
            var selectedCategory = await _categoryService.GetCategoryByIdAsync(Product.CategoryId); //Get Category for Product

            if (selectedCategory != null)
            {
                Product.CategoryName = selectedCategory.Name;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The selected category does not exist.");
                return Page();
            }

            try
            {
                await _productService.UpdateProductAsync(Product); //Update Product
            }
            catch (Exception ex)
            {
                var product = await _productService.GetProductByIdAsync(productId); //Reload Current Product

                if (product == null)
                {
                    return NotFound();//If Not Found
                }

                Product = product; //Reload the category with current database values
                return Page(); //Redisplay the form
            }

            return RedirectToPage("./Product");
        }
    }
}
