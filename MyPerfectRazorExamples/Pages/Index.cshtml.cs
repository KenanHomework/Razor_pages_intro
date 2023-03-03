using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPerfectRazorExamples.Models;
using MyPerfectRazorExamples.Services;

namespace MyPerfectRazorExamples.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ProductService _service;

        public IndexModel(ProductService service)
        {
            _service = service;
        }

        public void OnPost(Product product)
        {
            _service.AddProduct(product);
        }
    }
}