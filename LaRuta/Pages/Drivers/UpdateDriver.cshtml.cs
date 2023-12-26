using LaRuta.Data;
using LaRuta.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaRuta.Pages.Drivers
{
    public class UpdateDriverModel : PageModel
    {
        private readonly AppDbContext dbContext;

        [BindProperty]
        public DriverViewModel UpdateDriverRequest { get; set; }


        public UpdateDriverModel(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // metodo para obtener el chofer mediante el id (CI)
        public void OnGet(int CI)
        {
            // se busca el CI del chofer seleccionado en la DB
            var driver = dbContext.Drivers.Find(CI);

            if (driver != null)
            {
                // se crea un nuevo modelo de conductor a partir de los datos
                // ingresados en el formulario
                UpdateDriverRequest = new DriverViewModel
                {
                    CI = driver.CI,
                    FirstName = driver.FirstName,
                    LastName = driver.LastName,
                    Age = driver.Age,
                    Address = driver.Address,
                    LicenseNumber = driver.LicenseNumber,
                    LicenseExpiration = driver.LicenseExpiration,
                    VehicleId = driver.VehicleId,
                };
            }
            else
            {
                ViewData["Message"] = "No se encontró el chofer para actualizar.";
            }
        }

        // metodo para el boton de actualizar chofer
        public void OnPost()
        {
            // Verificar si el modelo creado en OnGet existe
            if (UpdateDriverRequest != null)
            {
                // Comprobar que los campos del formulario tienen valores correctos
                if (!ModelState.IsValid)
                {
                    ViewData["Error"] = "El chofer contiene errores. Por favor, corrige los errores e intenta nuevamente.";
                    return;
                }

                // verificar si el ID del vehiculo existe en la base de datos o si es nulo
                var vehicleExists = UpdateDriverRequest.VehicleId == null || dbContext.Vehicles.Any(v => v.CN == UpdateDriverRequest.VehicleId);

                // si es distinto de null pero no coincide con ningun registro, mostrar error
                if(!vehicleExists)
                {
                    ViewData["Error"] = "No se puede asignar un conductor a un vehiculo que no existe.";
                    return;
                }

                // Obtener el vehiculo con los datos originales a partir de su identificador
                var existingDriver = dbContext.Drivers.Find(UpdateDriverRequest.CI);

                // Reemplazar esos datos con los que se ingresaron en el formulario
                if (existingDriver != null)
                {
                    existingDriver.FirstName = UpdateDriverRequest.FirstName;
                    existingDriver.LastName = UpdateDriverRequest.LastName;
                    existingDriver.Age = UpdateDriverRequest.Age;
                    existingDriver.Address = UpdateDriverRequest.Address;
                    existingDriver.LicenseNumber = UpdateDriverRequest.LicenseNumber;
                    existingDriver.LicenseExpiration = UpdateDriverRequest.LicenseExpiration;

                    // verificar si el conductor ya tiene un vehiculo asignado
                    if (UpdateDriverRequest.VehicleId != null && dbContext.Drivers.Any(d => d.VehicleId == UpdateDriverRequest.VehicleId))
                    {
                        ViewData["Error"] = "El conductor ya tiene un vehículo asignado. Por favor, ingresa otro conductor.";
                        return;
                    }

                    // Obtener el vehículo actualmente asignado al conductor
                    var currentVehicle = dbContext.Vehicles.FirstOrDefault(v => v.DriverID == existingDriver.CI);

                    // Comprobar si el vehículo está siendo desasignado
                    if (UpdateDriverRequest.VehicleId == null)
                    {
                        // Desasignar el conductor del vehículo correspondiente
                        if (currentVehicle != null)
                        {
                            currentVehicle.DriverID = null;
                        }
                    }

                    // Asignar el vehiculo al conductor
                    existingDriver.VehicleId = UpdateDriverRequest.VehicleId;

                    // actualizar el vehicleID de la tabla de choferes
                    if(UpdateDriverRequest.VehicleId != null)
                    {
                        var updatedVehicle = dbContext.Vehicles.FirstOrDefault(v => v.CN == UpdateDriverRequest.VehicleId);
                        if (updatedVehicle != null)
                        {
                            updatedVehicle.DriverID = existingDriver.CI;
                        }
                    }

                    // Guardar info en la base de datos
                    dbContext.SaveChanges();
                    ViewData["Message"] = "Conductor actualizado correctamente!";
                }
                else
                {
                    ViewData["Message"] = "No se encontró el conductor para actualizar. Verifica el valor de CI proporcionado.";
                }
            }
        }
    }
}