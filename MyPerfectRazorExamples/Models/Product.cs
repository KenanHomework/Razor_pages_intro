using MyPerfectRazorExamples.TagHelpers;
using System.ComponentModel.DataAnnotations;

namespace MyPerfectRazorExamples.Models
{
    public class Product
    {
        [FormIgnore]
        public int Id { get; set; }

        [FormInput(Name = "Name", PlaceHolder = "Name here..", Tooltip = "Name of Product")]
        [Required, MinLength(4), MaxLength(128)]
        public string Name { get; set; } = string.Empty;


        [FormInput(Name = "Description", PlaceHolder = "Description here..", Tooltip = "Description of Product")]
        [Required, MinLength(1), MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        [FormIgnore]
        public uint Count { get; set; }

        [FormInput(Name = "Price", PlaceHolder = "Price here..", Tooltip = "Price of Product")]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        //[FormIgnore]
        public bool Available { get; set; }
    }
}
