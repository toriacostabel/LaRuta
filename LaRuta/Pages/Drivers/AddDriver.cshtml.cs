using LaRuta.Model.ViewModel;
using LaRuta.Model.Domain;
using LaRuta.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaRuta.Pages.Drivers
{
    public class AddDriverModel : PageModel
    {
        private readonly AppDbContext dbContext;

        [BindProperty]
        public DriverViewModel AddDriverRequest { get; set; }

        public AddDriverModel(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Chequea que todos los campos sean correctos
            if (ModelState.IsValid)
            {
                // Verificar si se proporcionó un VehicleId
                if (AddDriverRequest.VehicleId.HasValue)
                {
                    // Verificar si el VehicleId existe en la base de datos
                    var vehicleExists = dbContext.Vehicles.Any(v => v.CN == AddDriverRequest.VehicleId.Value);

                    // Verificar si el VehicleId está asignado a otro conductor
                    var vehicleAssignedToAnotherDriver = dbContext.Drivers.Any(d => d.VehicleId == AddDriverRequest.VehicleId.Value);

                    if (!vehicleExists)
                    {
                        ViewData["Error"] = "No se puede asignar un conductor a un vehículo que no existe.";
                        return;
                    }

                    if (vehicleAssignedToAnotherDriver)
                    {
                        ViewData["Error"] = "El vehículo ya está asignado a otro conductor.";
                        return;
                    }
                }

                var DriverDomainModel = new Driver
                {
                    CI = AddDriverRequest.CI,
                    FirstName = AddDriverRequest.FirstName,
                    LastName = AddDriverRequest.LastName,
                    Age = AddDriverRequest.Age,
                    Address = AddDriverRequest.Address,
                    LicenseNumber = AddDriverRequest.LicenseNumber,
                    LicenseExpiration = AddDriverRequest.LicenseExpiration,
                    VehicleId = AddDriverRequest.VehicleId,
                };

                dbContext.Drivers.Add(DriverDomainModel);
                dbContext.SaveChanges();
                ViewData["Message"] = "Chofer creado correctamente!";
            }
            else
            {
                ViewData["Error"] = "El formulario contiene errores. Por favor, corrige los errores e intenta nuevamente.";
            }
        }

    }
    }
            

       
