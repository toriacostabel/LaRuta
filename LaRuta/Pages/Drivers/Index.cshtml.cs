using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaRuta.Data;
using LaRuta.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace LaRuta.Pages.Drivers
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _driverContext;
        public IndexModel(AppDbContext driverContext)
        {
            _driverContext = driverContext ?? throw new ArgumentNullException(nameof(driverContext));
            Drivers = new List<Driver>();
        }

        public IEnumerable<Driver> Drivers { get; set; }

        // metodo para cargar todos los choferes en la lista
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
        }

        public async Task<IActionResult> OnPostDelete(int CI)
        {
            var driver = await _driverContext.Drivers.FindAsync(CI);

            if (driver == null)
            {
                ViewData["Message"] = "Chofer no encontrado.";
                return RedirectToPage();
            }

            if (driver.VehicleId.HasValue)
            {
                ViewData["Message"] = "No se puede eliminar a un chofer que tenga un vehiculo asignado.";
                return RedirectToPage();
            }

            _driverContext.Drivers.Remove(driver);
            await _driverContext.SaveChangesAsync();
            ViewData["Message"] = "Chofer eliminado correctamente!";
            return RedirectToPage();
        }
    }
}
