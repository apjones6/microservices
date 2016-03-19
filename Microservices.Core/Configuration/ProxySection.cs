using System;
using System.Configuration;

namespace Microservices.Core.Configuration
{
	public class ProxySection : ConfigurationSection
	{
		[ConfigurationProperty("proxies", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(ProxyElementCollection))]
		public ProxyElementCollection Proxies
		{
			get { return (ProxyElementCollection)this["proxies"]; }
			set { this["proxies"] = value; }
		}

		public ProxyOptions CreateOptionsForType(Type type)
		{
			foreach (ProxyElement proxy in Proxies)
			{
				if (proxy.Type == type)
				{
					return new ProxyOptions(proxy.BaseAddress);
				}
			}

			return null;
		}
	}
}
