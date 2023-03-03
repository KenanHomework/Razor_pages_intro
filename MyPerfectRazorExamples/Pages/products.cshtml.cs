using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPerfectRazorExamples.Models;
using MyPerfectRazorExamples.Services;

namespace MyPerfectRazorExamples.Pages
{
    public class productsModel : PageModel
    {
        private readonly ProductService _productService;
        public IEnumerable<Product> Products { get; private set; } = Enumerable.Empty<Product>();

        public productsModel(ProductService productService)
        {
            _productService = productService;
        }

        public async Task OnGet()
        {
            Products = await _productService.GetProductsAsync();
        }
    }
}
