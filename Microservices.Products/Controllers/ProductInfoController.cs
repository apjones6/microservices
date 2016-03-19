using Microservices.Products.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Microservices.Products.Controllers
{
	[RoutePrefix("api/products")]
    public class ProductInfoController : ApiController
    {
		private static readonly IEnumerable<Product> PRODUCTS = new[]
		{
			new Product(1, "Red sweater", 15.99),
			new Product(2, "Blue pens", 3.49),
			new Product(3, "Bubblegum", 0.99),
			new Product(4, "Apple", 0.20),
			new Product(5, "Batman Begins (Blu-ray)", 11.99)
		};
		
		[Route]
		public IHttpActionResult Get([FromUri] IEnumerable<int> productIds)
		{
			if (productIds == null || !productIds.Any())
			{
				return BadRequest("ProductIds parameter was missing or empty.");
			}

			var products = PRODUCTS.Where(x => productIds.Contains(x.Id)).ToArray();
			if (products.Length < productIds.Count())
			{
				return BadRequest("One or more products do not exist.");
			}

			return Ok(products);
		}
		
		[Route("{productId}")]
		public IHttpActionResult Get(int productId)
		{
			var product = PRODUCTS.SingleOrDefault(x => x.Id == productId);
			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}
	}
}
