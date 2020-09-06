using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using ServerAnnouncements.Api;

namespace ServerAnnouncements.Commands.Edit.Broadcast
{
	class Message : ICommand
	{
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			if (!sender.CheckPermission("sa.edit.broadcast.message", ref response)) return false;

			response = "Not implemented yet.";
			return false;
		}

		public string Command { get; } = "message";
		public string[] Aliases { get; } = {"m"};
		public string Description { get; } = "Edits the message of the announcement";
	}
}
