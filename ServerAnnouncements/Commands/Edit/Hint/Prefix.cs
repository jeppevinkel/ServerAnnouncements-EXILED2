using System;
using System.Collections.Generic;
using System.Text;
using CommandSystem;
using HarmonyLib;
using ServerAnnouncements.Api;

namespace ServerAnnouncements.Commands.Edit.Hint
{
	class Prefix : ParentCommand
	{
		private readonly IEnumerable<string> _prefixes;

		public Prefix(IEnumerable<string> prefixes)
		{
			this._prefixes = prefixes;

			LoadGeneratedCommands();
		}

		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new Message(_prefixes.AddItem(Command)));
		}

		protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			if (!sender.CheckPermission("sa.edit.hint", ref response)) return false;

			var sb = new StringBuilder("Available commands:\n");

			foreach (KeyValuePair<string, ICommand> command in this.Commands)
			{
				sb.AppendLine($"- {_prefixes.Join(null, " ")} {Command} {command.Value.Command} (Aliases: {command.Value.Aliases.Join()})");
			}

			response = sb.ToString();
			return false;
		}

		public override string Command { get; } = "hint";
		public override string[] Aliases { get; } = { "h" };
		public override string Description { get; } = "Lists available hint editing commands.";
	}
}