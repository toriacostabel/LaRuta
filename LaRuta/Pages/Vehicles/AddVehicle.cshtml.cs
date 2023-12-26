using LaRuta.Model.ViewModel;
using LaRuta.Model.Domain;
using LaRuta.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaRuta.Pages.Vehicles
{
    public class AddVehicleModel : PageModel
    {
        private readonly AppDbContext dbContext;

        [BindProperty]
        public VehicleViewModel AddVehicleRequest { get; set; }

        public AddVehicleModel(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // metodo para agregar un nuevo vehiculo
        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                // Verificar si se proporcionó un DriverID
                if (AddVehicleRequest.DriverID.HasValue)
                {
                    // Verificar si el DriverID existe en la base de datos
                    var driverExists = dbContext.Drivers.Any(d => d.CI == AddVehicleRequest.DriverID.Value);

                    // Verificar si el DriverID está asignado a otro vehículo
                    var driverAssignedToAnotherVehicle = dbContext.Vehicles.Any(v => v.DriverID == AddVehicleRequest.DriverID.Value);

                    if (!driverExists)
                    {
                        ViewData["Error"] = "No se puede asignar un vehículo a un conductor que no existe.";
                        return;
                    }

                    if (driverAssignedToAnotherVehicle)
                    {
                        ViewData["Error"] = "El conductor ya está asignado a otro vehículo.";
                        return;
                    }
                }

                var VehicleDomainModel = new Vehicle
                {
                    CN = AddVehicleRequest.CN,
                    LicensePlate = AddVehicleRequest.LicensePlate,
                    Brand = AddVehicleRequest.Brand,
                    Model = AddVehicleRequest.Model,
                    Year = AddVehicleRequest.Year,
                    DriverID = AddVehicleRequest.DriverID
                };

                // Asignar el ID del vehículo al atributo VehicleId del conductor
                if (AddVehicleRequest.DriverID.HasValue)
                {
                    var driver = dbContext.Drivers.Find(AddVehicleRequest.DriverID.Value);
                    if (driver != null)
                    {
                        driver.VehicleId = AddVehicleRequest.CN;
                    }
                }

                dbContext.Vehicles.Add(VehicleDomainModel);
                dbContext.SaveChanges();

                ViewData["Message"] = "Vehiculo creado correctamente!";
            }
            else
            {
                ViewData["Error"] = "El formulario contiene errores. Por favor, corrige los errores e intenta nuevamente.";
            }
        }
    }
}
