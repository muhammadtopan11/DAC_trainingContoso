using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModels
{
    public class StudentSearchVM : IValidatableObject
    {
        [Display(Name = "Nama Belakang")]
        public string LastName { get; set; }

        [Display(Name = "Nama Depan Tengah")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Pendaftaran Dari")]
        public DateTime? EnrollmentDateFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Pendaftaran Sampai")]
        public DateTime? EnrollmentDateUntil { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(LastName)
                && String.IsNullOrEmpty(FirstMidName))
            {
                yield return new ValidationResult("Masukan Minimal satu kolom pencarian");
            }

            if (EnrollmentDateFrom == null)
            {
                yield return new ValidationResult("Masukan Kolom Tanggal Dari!!", new[] { "EnrollmentDateFrom" });
            }

            if (EnrollmentDateUntil == null)
            {
                yield return new ValidationResult("Masukan Kolom Tanggal Sampai!!", new[] { "EnrollmentDateUntil" });
            }
        }
    }
}