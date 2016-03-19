using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Microservices.Core.Builders
{
	public class RequestBuilder : IProxyRequestBuilder
	{
		public RequestBuilder(MethodInfo methodInfo, object[] arguments)
		{
			Arguments = arguments;
			Method = methodInfo;
		}

		protected object[] Arguments { get; private set; }
		protected MethodInfo Method { get; private set; }

		public HttpRequestMessage Execute()
		{
			var request = new HttpRequestMessage();

			SetAcceptHeader(request);
			SetHttpMethod(request);
			SetRequestUri(request);

			return request;
		}

		protected virtual void SetAcceptHeader(HttpRequestMessage request)
		{
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		protected virtual void SetHttpMethod(HttpRequestMessage request)
		{
			var methodProvider = Method.GetCustomAttributes().OfType<IActionHttpMethodProvider>().FirstOrDefault();
			if (methodProvider != null)
			{
				request.Method = methodProvider.HttpMethods.First();
			}
		}

		protected virtual void SetRequestUri(HttpRequestMessage request)
		{
			// Find the template, use empty as the default
			var url = string.Empty;
			var routeAttribute = Method.GetCustomAttributes<RouteAttribute>().FirstOrDefault();
			if (routeAttribute != null)
			{
				url = routeAttribute.Template;
			}

			var paramNames = Method.GetParameters().Select(x => x.Name).ToArray();
			var excludeFromQs = new HashSet<int>();

			// Inject parameters into the template
			foreach (Match match in Regex.Matches(url, @"\{(?<name>[a-z]+)\}", RegexOptions.IgnoreCase))
			{
				var matchName = match.Groups["name"].Value;
				var i = Array.IndexOf(paramNames, matchName);
				if (i == -1)
				{
					throw new ApplicationException(string.Format("Method '{0}' does not contain parameter '{1}' for URL template '{2}'.", Method.Name, matchName, routeAttribute.Template));
				}

				// Replace the match value, as we want to replace the braces also. Give no
				// concession to enumerable arguments, but exclude from adding these parameters
				// to the query string
				url = url.Replace(match.Value, Arguments[i].ToString());
				excludeFromQs.Add(i);
			}

			// Build the querystring
			var qs = HttpUtility.ParseQueryString(string.Empty);
			for (var i = 0; i < Arguments.Length; i++)
			{
				// Skip excluded arguments
				if (excludeFromQs.Contains(i))
				{
					continue;
				}

				var arg = Arguments[i];
				var paramName = paramNames[i];

				// Don't push null onto the querystring
				if (arg == null)
				{
					continue;
				}
				else if (arg is IEnumerable)
				{
					foreach (var argItem in (IEnumerable)arg)
					{
						qs.Add(paramName, argItem.ToString());
					}
				}
				else
				{
					qs.Add(paramName, arg.ToString());
				}
			}

			if (qs.Count > 0)
			{
				url += "?" + qs.ToString();
			}

			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
		}
	}
}
