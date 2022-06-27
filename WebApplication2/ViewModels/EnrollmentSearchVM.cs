using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication2.Models;


namespace WebApplication2.ViewModels
{
    public class EnrollmentSearchVM : IValidatableObject
    {
        [Display(Name = "Judul")]
        public string Title { get; set; }

        [Display(Name = "Nama Belakang")]
        public string LastName { get; set; }

        [Display(Name = "Nilai Dari")]
        public Grade? GradeFrom { get; set; }

        [Display(Name = "Nilai Sampai")]
        public Grade? GradeUntil { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Grade? gradeFrom = GradeFrom;
            if (string.IsNullOrEmpty(LastName)
                && string.IsNullOrEmpty(Title))
            {
                yield return new ValidationResult("Masukan Minimal satu kolom pencarian");
            }

            if (GradeUntil == null)
            {
                yield return new ValidationResult("Masukan Kolom Nilai Dari!!", new[] { "GradeUntil" });
            }

            if (GradeFrom == null)
            {
                yield return new ValidationResult("Masukan Kolom Nilai Sampai!!", new[] { "GradeFrom" });
            }
        }
    }
}