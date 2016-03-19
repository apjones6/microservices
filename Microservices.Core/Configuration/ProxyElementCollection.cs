using System.Configuration;

namespace Microservices.Core.Configuration
{
	public class ProxyElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProxyElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ProxyElement)element).Type;
		}
	}
}
