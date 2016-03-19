using System.Collections.Generic;

namespace Microservices.Orders.Models
{
	public class Order
	{
		public Order()
		{
		}

		public Order(int id, IEnumerable<OrderItem> items)
		{
			this.Id = id;
			this.Items = items;
		}

		public int Id { get; set; }
		
		public IEnumerable<OrderItem> Items { get; set; }
	}
}