using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using ServerAnnouncements.Api;

namespace ServerAnnouncements.Commands
{
	class Reload : ICommand
	{
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			if (!sender.CheckPermission("sa.reload", ref response)) return false;

			ServerAnnouncements.ReloadAnnouncements();

			StringBuilder sb = new StringBuilder("Reloaded announcements:\n");
			sb.AppendLine($"    - Broadcasts: {ServerAnnouncements.Announcements.Broadcasts.Count}");
			sb.AppendLine($"    - Hints: {ServerAnnouncements.Announcements.Hints.Count}");

			response = sb.ToString();
			return true;
		}

		public string Command { get; } = "reload";
		public string[] Aliases { get; } = {"r"};
		public string Description { get; } = "Reloads the announcements from the config file.";
	}
}
