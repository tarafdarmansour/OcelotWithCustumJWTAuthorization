using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly List<Customer> _customers;

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
            var rng = new Random();

            _customers = Enumerable.Range(1, 10).Select(index => new Customer
            {
                BirthDate = DateTime.Now.AddYears(-1 * rng.Next(20,30)),
                FirstName = $"FirstName {index}",
                Id = index,
                LastName = $"LastName {index}"
            })
            .ToList();
        }

        [HttpGet]
        public IEnumerable<Customer> CustomerList()
        {
            return _customers;
        }

        [HttpGet("{id}")]
        public Customer Customer(int id)
        {
            return _customers.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
