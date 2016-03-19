using Microservices.Orders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microservices.Orders.Proxies
{
	public interface IProductInfoService
	{
		[Route("/api/products")]
		[HttpGet]
		Task<IEnumerable<Product>> GetAsync(IEnumerable<int> productIds);

		[Route("/api/products/{productId}")]
		[HttpGet]
		Task<Product> GetAsync(int productId);
	}
}
