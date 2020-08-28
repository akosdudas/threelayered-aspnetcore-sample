using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webshop.BL;
using Webshop.DAL;

namespace Webshop.Web.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerManager cm;

        public CustomerController(CustomerManager cm) => this.cm = cm;

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get() => await cm.ListCustomers();

        [HttpGet("{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Customer>> Get(int customerId)
        {
            var customer = await cm.GetCustomerOrNull(customerId);
            if (customer == null)
                return NotFound();
            else
                return Ok(customer);
        }

        [HttpDelete("{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Delete(int customerId)
        {
            var customer = await cm.GetCustomerOrNull(customerId);
            if (customer == null)
                return NotFound();
            else if (await cm.TryDeleteCustomer(customerId))
                return Ok();
            else
                return Conflict();
        }
    }
}