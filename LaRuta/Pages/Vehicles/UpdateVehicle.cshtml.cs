using LaRuta.Data;
using LaRuta.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaRuta.Pages.Vehicles
{
    public class UpdateVehicleModel : PageModel
    {
        private readonly AppDbContext dbContext;

        [BindProperty]
        public VehicleViewModel UpdateVehicleRequest { get; set; }


        public UpdateVehicleModel(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // metodo para obtener el vehiculo seleccionado de la lista del Index
        public void OnGet(int CN)
        {
            var vehicle = dbContext.Vehicles.Find(CN);

            // si el vehiculo existe, crear un nuevo modelo con los datos
            // ingresados en el formulario
            if (vehicle != null)
            {
                UpdateVehicleRequest = new VehicleViewModel
                {
                    CN = vehicle.CN,
                    LicensePlate = vehicle.LicensePlate,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    DriverID = vehicle.DriverID,
                };
            }
            else
            {
                ViewData["Message"] = "No se encontró el vehiculo para actualizar. Verifica el valor de CN proporcionado.";
            }
        }

        // metodo para el boton de actualizar vehiculo
        public void OnPost()
        {
            // Verificar si el modelo creado en OnGet existe
            if (UpdateVehicleRequest != null)
            {
                // Comprobar que los campos del formulario tienen valores correctos
                if (!ModelState.IsValid)
                {
                    ViewData["Error"] = "El vehículo contiene errores. Por favor, corrige los errores e intenta nuevamente.";
                    return;
                }

                // Verificar si el ID del conductor existe en la base de datos o si es nulo
                var driverExists = UpdateVehicleRequest.DriverID == null || dbContext.Drivers.Any(d => d.CI == UpdateVehicleRequest.DriverID);

                // Si es distinto de null pero no coincide con ningún registro, mostrar error
                if (!driverExists)
                {
                    ViewData["Error"] = "No se puede asignar un vehículo a un conductor que no existe.";
                    return;
                }

                // Obtener el vehículo con los datos originales a partir de su identificador
                var existingVehicle = dbContext.Vehicles.Find(UpdateVehicleRequest.CN);

                // Reemplazar esos datos con los que se ingresaron en el formulario
                if (existingVehicle != null)
                {
                    existingVehicle.LicensePlate = UpdateVehicleRequest.LicensePlate;
                    existingVehicle.Brand = UpdateVehicleRequest.Brand;
                    existingVehicle.Model = UpdateVehicleRequest.Model;
                    existingVehicle.Year = UpdateVehicleRequest.Year;

                    if (UpdateVehicleRequest.DriverID != null && dbContext.Vehicles.Any(v => v.DriverID == UpdateVehicleRequest.DriverID))
                    {
                        ViewData["Error"] = "El conductor ya tiene un vehículo asignado. Por favor, ingresa otro conductor.";
                        return;
                    }
                    // Obtener el conductor actualmente asignado al vehículo
                    var currentDriver = dbContext.Drivers.FirstOrDefault(d => d.VehicleId == existingVehicle.CN);

                    // Comprobar si el conductor está siendo desasignado
                    if (UpdateVehicleRequest.DriverID == null)
                    {
                        // Desasignar el vehículo del conductor correspondiente
                        if (currentDriver != null)
                        {
                            currentDriver.VehicleId = null;
                        }
                    }

                    // Asignar el conductor al vehículo
                    existingVehicle.DriverID = UpdateVehicleRequest.DriverID;

                    // actualizar el driver id de la tabla de vehiculos
                    if (UpdateVehicleRequest.DriverID != null)
                    {
                        var updatedDriver = dbContext.Drivers.FirstOrDefault(d => d.CI == UpdateVehicleRequest.DriverID);
                        if(updatedDriver != null) 
                        {
                            updatedDriver.VehicleId = existingVehicle.CN;
                        }
                    }

                    // Guardar info en la base de datos
                    dbContext.SaveChanges();
                    ViewData["Message"] = "Vehículo actualizado correctamente!";
                }
                else
                {
                    ViewData["Message"] = "No se encontró el vehículo para actualizar. Verifica el valor de CN proporcionado.";
                }
            }
        }
    }

}
