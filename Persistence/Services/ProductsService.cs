using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Services
{
    //Service to Handle all Calls from "Product" Pages
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;

        public ProductsService(ApplicationDbContext context)
        {
            _context = context; //Get DB
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync(); //Get All Products
        }

        public async Task<Products> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id); //Get Specific Product
        }

        public async Task AddProductAsync(Products product)
        {
            _context.Products.Add(product); //Add Product to DB
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Products product)
        {
            try
            {
                _context.Products.Update(product); //Update Product
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(product.CategoryId)) //Check if Product doesn't Exist
                {
                    throw new KeyNotFoundException($"Product with ID {product.CategoryId} not found.");
                }
                else
                {
                    //Make sure only one Person editing Records at a Time
                    throw new DbUpdateConcurrencyException("The record you attempted to edit was modified by another user after you got the original value.");
                }
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product); //Delete Product
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetProductImageByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id); //Get Image Associated with Product
            return product?.ImageUrl;
        }

        public async Task<List<Categories>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync(); //Get All Categories
        }

        public async Task<string> GenerateProductCodeAsync()
        {
            var currentYearMonth = DateTime.Now.ToString("yyyyMM"); //Format Product Code String
            var lastProduct = await _context.Products
                                .OrderByDescending(p => p.ProductId)
                                .FirstOrDefaultAsync(); //Get Last Inserted Product

            int nextSequence = 1;
            if (lastProduct != null)
            {
                var lastCode = lastProduct.ProductCode.Substring(7); //Get Previous Product Code
                int.TryParse(lastCode, out nextSequence);
                nextSequence++; //Update Sequence Counter
            }

            return $"{currentYearMonth}-{nextSequence:D3}"; 
        }

        public async Task<string> GetCategoryNameByIdAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            return category?.Name; //Get Category Name
        }

        private async Task<bool> ProductExists(int productId)
        {
            return await _context.Products.AnyAsync(e => e.ProductId == productId); //Check if Product Exists
        }

    }
}


