using System.ComponentModel.DataAnnotations;

namespace LaRuta.Model.Domain
{
    public class Driver
    {
        [Key]
        [MaxLength(8)]
        [Required(ErrorMessage = "La Cédula de Identidad es obligatoria.")]
        public int CI { get; set; } // 8-digit, PK

        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [Display(Name = "Nombre: ")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "El Apellido es obligatorio.")]
        [Display(Name = "Apellido: ")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "La Edad es obligatoria.")]
        [Display(Name = "Edad: ")]
        public int Age { get; set; }

        [Required(ErrorMessage = "La Dirección es obligatoria.")]
        [Display(Name = "Direccion: ")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "El Número de licencia es obligatorio.")]
        [Display(Name = "Licencia: ")]
        public string? LicenseNumber { get; set; }

        [Required(ErrorMessage = "La Fecha de expiración de licencia es obligatoria.")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Vencimiento: ")]
        public DateTime LicenseExpiration { get; set; }

        [Display(Name = "Vehiculo: ")]
        public int? VehicleId { get; set; }

        public Driver()
        {

        }


    }
}
