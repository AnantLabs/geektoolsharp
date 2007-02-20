using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;

namespace GeekTool
{
	/// <summary>
	/// InstancesHandler is a class that allows you to read customized application configuration.
	/// Supposedly this interface is depreciated, but using ConfigurationSection 
	/// doesn't allow me to use a name for an element more than once.
	/// </summary>
	public class InstancesHandler : IConfigurationSectionHandler
	{
		public InstancesHandler()
		{
		}

		/// <summary>
		/// This method implements the IConfigurationSectionHandler interface.
		/// This interface defines the contract that all configuration section handlers must 
		/// implement in order to participate in the resolution of configuration settings.
		/// </summary>
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			List<Instance> list = new List<Instance>();

			foreach (XmlNode node in section.SelectNodes(Constants.Instance))
			{
				XmlNodeReader xmlNodeReader = new XmlNodeReader(node);
				XmlSerializer serializer = new XmlSerializer(typeof(Instance));
				Instance instance = (Instance)serializer.Deserialize(xmlNodeReader);

				list.Add(instance);
			}

			return list;
		}
	}
}
