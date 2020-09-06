using System.ComponentModel;
using Exiled.API.Interfaces;

namespace ServerAnnouncements
{
	public class Config : IConfig
	{
		[Description("Enables the plugin.")] public bool IsEnabled { get; set; } = true;
	}
}
