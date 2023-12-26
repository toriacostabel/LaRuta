using LaRuta.Data;
using LaRuta.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaRuta.Pages.Drivers
{
    public class DeleteDriverModel : PageModel
    {
        private readonly AppDbContext dbContext;

        public DeleteDriverModel(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public int DriverIdToDelete { get; set; }

        [BindProperty]
        public DriverViewModel DeleteDriverRequest { get; set; }

        public void OnGet()
        {
            // You may implement code here to populate the page with the current driver details for confirmation.
        }

        public void OnPost()
        {
            var driverToDelete = dbContext.Drivers.Find(DriverIdToDelete);

            if (driverToDelete != null)
            {
                // Check if VehicleId is null or empty before deleting
                if (string.IsNullOrEmpty(driverToDelete.VehicleId.ToString()))
                {
                    dbContext.Drivers.Remove(driverToDelete);
                    dbContext.SaveChanges();
                    ViewData["Message"] = "Chofer eliminado correctamente!";
                }
                else
                {
                    ViewData["Message"] = "No se puede eliminar el chofer porque tiene un vehículo asignado.";
                }
            }
            else
            {
                ViewData["Message"] = "No se encontró el chofer para eliminar.";
            }
        }
    }
}
