using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Devices.Boundary
{
    public class DeviceController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}