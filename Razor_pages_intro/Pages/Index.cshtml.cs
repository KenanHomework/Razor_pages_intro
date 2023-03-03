using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text;

namespace Razor_pages_intro.Pages
{
    public class IndexModel : PageModel
    {
        //public string MyMessage { get; set; }
        public string foo()
        {
            return "Foo";
        }
        //public void OnGet(string name, int age)
        //{
        //    //ViewData["Name"] = name;
        //    //ViewData["Age"] = age;
        //    MyMessage = $"{name} {age}";
        //}

        //public void OnGet(Person person)
        //{
        //    MyMessage = $"From object {person.Name} {person.Age}";
        //}


        //public void OnGet(string[] names)
        //{
        //    var result = new StringBuilder();
        //    foreach (var name in names)
        //    {
        //        result.Append($"{name} ");
        //    }
        //    MyMessage = result.ToString();
        //}

        //public ContentResult OnGet()
        //{
        //    return Content("Content method used");
        //}


        //private readonly IWebHostEnvironment _environment;

        //public IndexModel(IWebHostEnvironment environment)
        //{
        //    _environment = environment;
        //}

        //public FileResult OnGet()
        //{
        //    string path = Path.Combine(_environment.ContentRootPath, "Files/file.txt");
        //    var bytes = System.IO.File.ReadAllBytes(path);
        //    return File(bytes, "Application/txt", "Pile.txt");
        //}



        //public ForbidResult OnGet()
        //{
        //    return Forbid();
        //}

        //public NotFoundResult OnGet()
        //{
        //    return NotFound();
        //}
    }
}