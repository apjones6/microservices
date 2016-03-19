using System.Net.Http;
using System.Reflection;

namespace Microservices.Core.Builders
{
	public interface IProxyRequestBuilder
	{
		HttpRequestMessage Execute();
	}
}
