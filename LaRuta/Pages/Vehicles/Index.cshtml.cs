using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaRuta.Data;
using LaRuta.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace LaRuta.Pages.Vehicles
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _vehicleContext;

        public IEnumerable<Vehicle> Vehicles { get; set; }

        public IndexModel(AppDbContext vehicleContext)
        {
            _vehicleContext = vehicleContext ?? throw new ArgumentNullException(nameof(vehicleContext));
            Vehicles = new List<Vehicle>();
        }

        // metodo para traer todos los vehiculos de la DB
        public async Task OnGet()
        {
            Vehicles = await _vehicleContext.Vehicles.ToListAsync();
        }
        public async Task<IActionResult> OnPostDelete(int CN)
        {
            var vehicle = await _vehicleContext.Vehicles.FindAsync(CN);

            if (vehicle == null)
            {
                ViewData["Message"] = "Vehiculo no encontrado.";
                return RedirectToPage();
            }

            if (vehicle.DriverID.HasValue)
            {
                return RedirectToPage();
            }

            _vehicleContext.Vehicles.Remove(vehicle);
            await _vehicleContext.SaveChangesAsync();
            ViewData["Message"] = "Vehiculo eliminado correctamente!";
            return RedirectToPage();
        }
    }
}
