using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private static readonly string[] _productSampleList = new[]
        {
            "Metex","Altus Car service","Design bot","De Leen","Green Bay","Greenspam","Aristo GLory","Green Frog","Doodle","Districts"
        };

        private static readonly string[] _brandsSampleList = new[]
        {
            "DewDrop","Genext","Granite","Guru Hopper","Magma","Ayush","Greenspan","Trust Inside","Mimimal","Guru Logic","Archstone","Goldcrest","Flex Gel","Aplomb"
        };

        private readonly List<Product> _products;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
                var rng = new Random();
            int productCount = _productSampleList.Count();
            int brandCount = _brandsSampleList.Count();
            _products=  Enumerable.Range(1, productCount).Select(index => new Product
            {
                Id = index,
                Brand = _brandsSampleList[rng.Next(brandCount)],
                DiscountRate = rng.Next(5,brandCount),
                Name = _productSampleList[rng.Next(productCount)]
            })
            .ToList();
        }

        [HttpGet]
        public IEnumerable<Product> Products()
        {
            return _products;
        }

        [HttpGet("{id}")]
        public Product Product(int id)
        {
            return _products.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
