using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace ServerAnnouncements.Api
{
	public static class Extensions
	{
		public static bool CheckPermission(this ICommandSender sender, string permission, ref string response)
		{
			if (sender.CheckPermission(permission)) return true;
			response = $"Your do not have the required permission to use this command. ({permission})";
			return false;

		}
	}
}
