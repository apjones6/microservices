using Castle.DynamicProxy;
using Microservices.Core.Configuration;
using System;
using System.Configuration;

namespace Microservices.Core
{
    public class ProxyFactory
    {
		private static readonly ProxyGenerator generator = new ProxyGenerator();

		public static T Create<T>()
			where T : class
		{
			var section = ConfigurationManager.GetSection("proxy") as ProxySection;
			if (section == null)
			{
				throw new InvalidOperationException("Configuration section could not be found.");
			}

			var type = typeof(T);
			var options = section.CreateOptionsForType(type);
			if (options == null)
			{
				throw new InvalidOperationException(string.Format("Configuration proxy for type '{0}' could not be found.", type.AssemblyQualifiedName));
			}

			return Create<T>(options);
		}

		public static T Create<T>(ProxyOptions options)
			where T : class
		{
			var interceptor = new Proxy(options);
			var proxy = generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
			return proxy;
		}
    }
}
