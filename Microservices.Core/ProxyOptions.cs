using System;

namespace Microservices.Core
{
	public class ProxyOptions
	{
		public ProxyOptions()
		{
		}

		public ProxyOptions(string baseAddress)
		{
			BaseAddress = new Uri(baseAddress);
		}

		public Uri BaseAddress { get; set; }
	}
}
