using System;
using System.ComponentModel;
using System.Configuration;

namespace Microservices.Core.Configuration
{
	public class ProxyElement : ConfigurationElement
	{
		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
		[TypeConverter(typeof(TypeNameConverter))]
		public Type Type
		{
			get { return (Type)this["type"]; }
			set { this["type"] = value; }
		}

		[ConfigurationProperty("baseAddress", IsRequired = true)]
		//[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
		public string BaseAddress
		{
			get { return (string)this["baseAddress"]; }
			set { this["baseAddress"] = value; }
		}
	}
}
