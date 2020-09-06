using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using ServerAnnouncements.Api;

namespace ServerAnnouncements.Commands.Edit.Hint
{
	class Message : ICommand
	{
		private readonly IEnumerable<string> _prefixes;

		public Message(IEnumerable<string> prefixes)
		{
			_prefixes = prefixes;
		}

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			if (!sender.CheckPermission("sa.edit.hint.message", ref response)) return false;

			response = "Not implemented yet.";
			return false;
		}

		public string Command { get; } = "message";
		public string[] Aliases { get; } = {"m"};
		public string Description { get; } = "Edits the message of the announcement";
	}
}
