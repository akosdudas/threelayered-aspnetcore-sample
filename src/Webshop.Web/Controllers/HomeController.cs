using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Webshop.BL;

namespace Webshop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly CustomerManager cm;

        public HomeController(CustomerManager cm) => this.cm = cm;

        public async Task<IActionResult> Index() => View(await cm.ListCustomers());
    }
}
