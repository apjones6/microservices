using Castle.DynamicProxy;
using Microservices.Core.Builders;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Microservices.Core
{
	public class Proxy : IInterceptor
	{
		private readonly HttpClient client;

		public Proxy(ProxyOptions options)
		{
			client = new HttpClient
			{
				BaseAddress = options.BaseAddress
			};
		}
		
		public void Intercept(IInvocation invocation)
		{
			var rtnType = invocation.Method.ReturnType;
			if (rtnType == typeof(Task) || (rtnType.IsGenericType && rtnType.GetGenericTypeDefinition() == typeof(Task<>)))
			{
				// Asynchronous
				invocation.ReturnValue = ConvertTask(ExecuteAsync(invocation), rtnType);
			}
			else
			{
				// Synchronous
				var result = ExecuteAsync(invocation).Result;
				if (rtnType != typeof(void))
				{
					invocation.ReturnValue = result;
				}
			}
		}
		
		private async Task<object> ExecuteAsync(IInvocation invocation)
		{
			var builder = new RequestBuilder(invocation.Method, invocation.Arguments);
			var request = builder.Execute();

			var response = await client.SendAsync(request);
			
			var rtnType = invocation.Method.ReturnType;
			if (rtnType != typeof(Task))
			{
				var content = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject(content, rtnType.GenericTypeArguments[0]);
			}

			return null;
		}
		
		private static Task ConvertTask(Task<object> task, Type type)
		{
			// If the output doesn't include a result, or our result is correct, return the input task
			if (type == typeof(Task) || task.GetType() == type)
			{
				return task;
			}

			// Convert the Task<object> into Task<T>, using the provided type
			var convert = typeof(Proxy).GetMethod("Convert", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(type.GenericTypeArguments);
			return (Task)convert.Invoke(null, new[] { task });
		}

		private async static Task<T> Convert<T>(Task<object> task)
		{
			var result = await task;
			return (T)result;
		}
	}
}
