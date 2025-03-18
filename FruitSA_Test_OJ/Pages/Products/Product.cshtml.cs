using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Persistence.Services;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace FruitSA_Test_OJ.Pages.Products.Product
{
    //Page to Display All Products
    public class ProductsModel : PageModel
    {
        private readonly IProductsService _productService;

        public ProductsModel(IProductsService productService)
        {
            _productService = productService; //Get Service
        }

        public IList<Domain.Entities.Products> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = (IList<Domain.Entities.Products>)await _productService.GetProductsAsync(); //List Products
        }

        [HttpPost]
        [Route("/OnGetDownloadAsync")]
        public async Task<IActionResult> OnGetDownloadAsync() //Dosnload Products
        {
            var products = await _productService.GetProductsAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //Set License of EPPlus

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products"); //Set Worksheet Name
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Medium9); //Load Products

                // Format the header row
                worksheet.Cells[1, 1, 1, 5].Style.Font.Bold = true; 

                var stream = new MemoryStream();
                package.SaveAs(stream); //Save Details
                stream.Position = 0;

                var fileName = "Products.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); //Download File
            }

        }
    }
}
