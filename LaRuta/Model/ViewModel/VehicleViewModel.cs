using System.ComponentModel.DataAnnotations;

namespace LaRuta.Model.ViewModel
{
    public class VehicleViewModel : IValidatableObject
    {
        public int CN { get; set; }

        // la matricula se compone de 3 letras y 4 numeros
        // no se agrega un espacio para la comodidad del usuario

        [RegularExpression("^[A-Za-z]{3}\\d{4}$", ErrorMessage = "El formato de la matrícula es incorrecto.")]
        public string? LicensePlate { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public int? DriverID { get; set; }


        // validacion para que el año del auto no pueda ser menor al año del primer auto ni mayor al año que viene
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Year < 1886 || Year > DateTime.Now.Year + 1)
            {
                yield return new ValidationResult("El año del vehículo debe ser igual o mayor que 1886, e igual o menor que el año entrante.", new[] { nameof(Year) });
            }
        }
    }

    
}
