using Microsoft.AspNetCore.Mvc.RazorPages;
using Persistence.Services;

namespace FruitSA_Test_OJ.Pages.Categories.Category
{
    //Page to Display All Categories
    public class CategoryModel : PageModel
    {
        private readonly ICategoriesService _categoryService;
        public CategoryModel(ICategoriesService categoryService)
        {
            _categoryService = categoryService; //Get Service
        }

        public IList<Domain.Entities.Categories> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = (IList<Domain.Entities.Categories>)await _categoryService.GetCategoriesAsync(); //List Categories
        
        }
    }
}
