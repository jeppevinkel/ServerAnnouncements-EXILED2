using System;
using System.Collections.Generic;
using System.Text;
using CommandSystem;
using HarmonyLib;
using ServerAnnouncements.Api;

namespace ServerAnnouncements.Commands
{
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	class Prefix : ParentCommand
	{
		public Prefix() => LoadGeneratedCommands();

		public override void LoadGeneratedCommands()
		{
			RegisterCommand(new Reload());

			RegisterCommand(new Edit.Prefix(new List<string>{Command}));
		}

		protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = null;
			if (!sender.CheckPermission("sa", ref response)) return false;

			var sb = new StringBuilder("Available commands:\n");

			foreach (KeyValuePair<string, ICommand> command in this.Commands)
			{
				sb.AppendLine($"- {Command} {command.Value.Command} (Aliases: {command.Value.Aliases.Join()})");
			}

			response = sb.ToString();
			return false;
		}

		public override string Command { get; } = "sa";
		public override string[] Aliases { get; } = { "serverannouncements" };
		public override string Description { get; } = "Handles commands related to server announcements.";
	}
}
