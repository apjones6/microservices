namespace Microservices.Orders.Models
{
	public class OrderItem : Product
	{
		public OrderItem()
		{
		}

		public OrderItem(int productId, int count)
		{
			this.Count = count;
			this.Id = productId;
		}

		public OrderItem(Product product, int count)
			: base(product)
		{
			this.Count = count;
		}

		public int Count { get; set; }
	}
}