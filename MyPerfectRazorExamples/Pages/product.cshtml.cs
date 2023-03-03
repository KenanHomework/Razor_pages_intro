using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPerfectRazorExamples.Models;
using MyPerfectRazorExamples.Services;

namespace MyPerfectRazorExamples.Pages
{
    public class productModel : PageModel
    {

        private readonly ProductService _productService;
        public Product? Product { get; private set; }

        public productModel(ProductService productService)
        {
            _productService = productService;
        }

        public async void OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id);
        }
    }
}
