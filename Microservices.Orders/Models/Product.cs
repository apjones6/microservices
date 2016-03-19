namespace Microservices.Orders.Models
{
	public class Product
	{
		public Product()
		{
		}

		public Product(int id, string name, double price)
		{
			this.Id = id;
			this.Name = name;
			this.Price = price;
		}

		public Product(Product source)
		{
			this.Id = source.Id;
			this.Name = source.Name;
			this.Price = source.Price;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public double Price { get; set; }
	}
}