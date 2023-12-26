using LaRuta.Model.Domain;
using LaRuta.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LaRuta.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _vehicleContext;
        private readonly AppDbContext _driverContext;



        public IndexModel(
            AppDbContext vehicleContext, AppDbContext driverContext)
        {
            _vehicleContext = vehicleContext ?? throw new ArgumentNullException(nameof(vehicleContext));
            _driverContext = driverContext ?? throw new ArgumentNullException(nameof(driverContext));

            Drivers = new List<Driver>();
            Vehicles = new List<Vehicle>();
        }

        public IEnumerable<Driver> Drivers { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }

        // metodo para cargar todos los datos al abrir la a
        public async Task OnGet()
        {
            Drivers = await _driverContext.Drivers
             .Select(d => new Driver
             {
                 CI = d.CI,
                 FirstName = d.FirstName,
                 LastName = d.LastName,
                 Age = d.Age,
                 Address = d.Address,
                 LicenseNumber = d.LicenseNumber,
                 LicenseExpiration = d.LicenseExpiration,
                 VehicleId = d.VehicleId ?? null
             })
             .ToListAsync();
            Vehicles = await _vehicleContext.Vehicles
                .Select(d => new Vehicle
                {
                    CN = d.CN,
                    LicensePlate = d.LicensePlate,
                    Brand = d.Brand,
                    Model = d.Model,
                    Year = d.Year,
                    DriverID = d.DriverID ?? null
                })
                .ToListAsync();
        }

        // metodo para filtrar los vehiculos que no tengan un chofer asignado
        public IActionResult OnPostShowUnassignedVehicles()
        {
            Vehicles = _vehicleContext.Vehicles.Where(v => v.DriverID == null).ToList();
            LoadDrivers();
            return Page();
        }

        // metodo para mostrar el vehiculo mas antiguo
        public IActionResult OnPostShowOldestVehicle()
        {
            var oldestVehicle = _vehicleContext.Vehicles
                .Where(v => v.Year != null)
                .OrderBy(v => v.Year)
                .FirstOrDefault();

            if (oldestVehicle != null)
            {
                Vehicles = new List<Vehicle> { oldestVehicle };
            }
            LoadDrivers();
            return Page();
        }

        // metodo para mostrar todos los conductores aunque se filtren los vehiculos
        private void LoadDrivers()
        {
            Drivers = _driverContext.Drivers.ToList();
        }

        // metodo para volver a mostrar todos los vehiculos sin filtrar
        public IActionResult OnPostShowAllVehicles()
        {
            LoadData();
            return Page();
        }


        // cargar todos los registros de la base de datos, sin filtros
        private void LoadData()
        {
            Drivers = _driverContext.Drivers
                .Select(d => new Driver
                {
                    CI = d.CI,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Age = d.Age,
                    Address = d.Address,
                    LicenseNumber = d.LicenseNumber,
                    LicenseExpiration = d.LicenseExpiration,
                    VehicleId = d.VehicleId ?? null
                })
                .ToList();
            Vehicles = _vehicleContext.Vehicles
                .Select(d => new Vehicle
                {
                    CN = d.CN,
                    LicensePlate = d.LicensePlate,
                    Brand = d.Brand,
                    Model = d.Model,
                    Year = d.Year,
                    DriverID = d.DriverID ?? null
                })
                .ToList();
        }
    }
}
