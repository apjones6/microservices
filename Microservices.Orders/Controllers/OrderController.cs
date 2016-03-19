using Microservices.Core;
using Microservices.Orders.Models;
using Microservices.Orders.Proxies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microservices.Orders.Controllers
{
	[RoutePrefix("api/orders")]
    public class OrderController : ApiController
	{
		private static readonly IEnumerable<Order> ORDERS = new[]
		{
			new Order(1, new[] { new OrderItem(1, 2), new OrderItem(4, 5) }),
			new Order(2, new[] { new OrderItem(1, 3) }),
			new Order(3, new[] { new OrderItem(5, 1) })
		};

		private readonly IProductInfoService productInfoService;
		
		public OrderController()
		{
			productInfoService = ProxyFactory.Create<IProductInfoService>();
		}

		[Route("{orderId}")]
		public async Task<IHttpActionResult> Get(int orderId)
		{
			var order = ORDERS.SingleOrDefault(x => x.Id == orderId);
			if (order == null)
			{
				return NotFound();
			}

			if (productInfoService != null)
			{
				var products = await productInfoService.GetAsync(order.Items.Select(x => x.Id));
				order.Items = order.Items
					.Select(x => new OrderItem(products.Single(p => p.Id == x.Id), x.Count))
					.ToArray();
			}

			return Ok(order);
		}

		[Route("{orderId}/items/{productId}")]
		public async Task<IHttpActionResult> Get(int orderId, int productId)
		{
			var order = ORDERS.SingleOrDefault(x => x.Id == orderId);
			if (order == null)
			{
				return NotFound();
			}

			var orderItem = order.Items.SingleOrDefault(x => x.Id == productId);
			if (orderItem == null)
			{
				return NotFound();
			}

			if (productInfoService != null)
			{
				var product = await productInfoService.GetAsync(orderItem.Id);
				orderItem = new OrderItem(product, orderItem.Count);
			}

			return Ok(orderItem);
		}
	}
}
