using System.ComponentModel.DataAnnotations;

namespace LaRuta.Model.ViewModel
{
    public class DriverViewModel : IValidatableObject
    {

        public int CI { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
        public string? LicenseNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime LicenseExpiration { get; set; }
        public int? VehicleId { get; set; }

        // validacion para el campo de fecha de expiracion de la licencia de conducir
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LicenseExpiration <= DateTime.Now)
            {
                yield return new ValidationResult("La fecha de expiración de la licencia debe ser mayor que la fecha actual.", new[] { nameof(LicenseExpiration) });
            }
        }
    }


}
