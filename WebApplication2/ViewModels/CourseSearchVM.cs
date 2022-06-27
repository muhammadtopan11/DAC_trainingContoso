using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModels
{
    public class CourseSearchVM : IValidatableObject
    {
        [Display(Name = "Judul")]
        public string Title { get; set; }

        [Display(Name = "Credits")]
        public int Credits { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Title)
                && Credits == 0)
            {
                yield return new ValidationResult("Masukan Minimal satu kolom pencarian");
            }

        }
    }
}