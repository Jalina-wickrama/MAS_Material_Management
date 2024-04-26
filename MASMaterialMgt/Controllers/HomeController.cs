using MASMaterialMgt.Areas.Identity.Data;
using MASMaterialMgt.Data;
using MASMaterialMgt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MASMaterialMgt.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MASDbContext context;
        private readonly IWebHostEnvironment environment;

        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, MASDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;

            _logger = logger;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            var totalMaterialCount = context.Materials.Count();
            var outOfStockMaterialCount = context.Materials.Count(m => m.Qty < 0);
            var activeSuppliersCount = context.Suppliers.Count(s => s.Activity == "Active");
            var deactivatedSuppliersCount = context.Suppliers.Count(s => s.Activity == "Deactive");

            ViewBag.TotalMaterialCount = totalMaterialCount;
            ViewBag.OutOfStockMaterialCount = outOfStockMaterialCount;
            ViewBag.ActiveSuppliersCount = activeSuppliersCount;
            ViewBag.DeactivatedSuppliersCount = deactivatedSuppliersCount;

            // Get Material data for bar chart
            var materials = context.Materials.ToList();
            ViewBag.MaterialNames = materials.Select(m => m.Name).ToList();
            ViewBag.MaterialQuantities = materials.Select(m => m.Qty).ToList();

            // Get Supplier data for activity bar chart
            var activeSupplierCount = context.Suppliers.Count(s => s.Activity == "Active");
            var deactiveSupplierCount = context.Suppliers.Count(s => s.Activity == "Deactive");

            ViewBag.SupplierActivityLabels = new List<string> { "Active", "Deactive" };
            ViewBag.SupplierActivityCounts = new List<int> { activeSupplierCount, deactiveSupplierCount };

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
