using System.ComponentModel.DataAnnotations;

namespace LaRuta.Model.Domain
{
    public class Vehicle
    {
        [Key]
        [MaxLength(5)]
        public int CN { get; set; } // 5-digit, PK

        [Required]
        [Display(Name = "Matricula: ")]
        public string? LicensePlate { get; set; } // 3 letters, 1 space, 4 numbers

        [Required]
        [Display(Name = "Marca: ")]
        public string? Brand { get; set; }

        [Required]
        [Display(Name = "Modelo: ")]
        public string? Model { get; set; }

        [Required]
        [Display(Name = "Año: ")]
        public int Year { get; set; } // 4-digit

        [Display(Name = "Chofer: ")]
        [MaxLength(8)]
        public int? DriverID { get; set; }

        public Vehicle()
        {

        }
    }
}
