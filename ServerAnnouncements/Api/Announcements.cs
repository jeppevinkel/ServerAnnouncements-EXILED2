using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ServerAnnouncements.Api.YamlComments;
using YamlDotNet.Serialization;

namespace ServerAnnouncements.Api
{
	public class Announcements
	{
		public static readonly string PluginPath =
			Environment.ExpandEnvironmentVariables(@"%AppData%\EXILED\Plugins\ServerAnnouncements");
		public static readonly string dataPath = Path.Combine(PluginPath, "data.yml");

		[Description("Broadcasts are shown at the top of the screen.")]
		public List<Broadcast> Broadcasts { get; internal set; } = new List<Broadcast>();

		[Description("Hints are shown as smaller text at the bottom of the screen.")]
		public List<Hint> Hints { get; internal set; } = new List<Hint>();

		public void SaveData()
		{
			ISerializer serializer = new SerializerBuilder()
				.WithTypeInspector(inner => new CommentGatheringTypeInspector(inner))
				.WithEmissionPhaseObjectGraphVisitor(args => new CommentsObjectGraphVisitor(args.InnerVisitor))
				.Build();
			string yaml = serializer.Serialize(this);

			Directory.CreateDirectory(PluginPath);

			File.WriteAllText(dataPath, yaml);
		}

		public static void LoadData()
		{
			if (!File.Exists(dataPath))
			{
				ServerAnnouncements.Announcements = new Announcements();
				ServerAnnouncements.Announcements.Broadcasts = new List<Broadcast>
				{
					new Broadcast(20, 6, 30, $"You are playing on {Server.Name}. Please enjoy your stay!")
				};
				ServerAnnouncements.Announcements.Hints = new List<Hint>
				{
					new Hint(40, 3, 40, "Have a nice day!"),
					new Hint(40, 3, 60, "Enjoy your stay!")
				};

				ServerAnnouncements.Announcements.SaveData();

				return;
			}
			string data = File.ReadAllText(dataPath);

			if (string.IsNullOrEmpty(data))
			{
				ServerAnnouncements.Announcements = new Announcements();
				ServerAnnouncements.Announcements.SaveData();

				return;
			}

			IDeserializer deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().IgnoreFields().Build();

			var announcements = deserializer.Deserialize<Announcements>(data);

			ServerAnnouncements.Announcements = announcements;
		}
	}

	public struct Broadcast
	{
		[Description("Amount of time between the broadcast being displayed in seconds.")]
		public float Interval { get; internal set; }
		[Description("How long to show the broadcast in seconds.")]
		public ushort Duration { get; internal set; }
		[Description("How long in seconds to wait before initially displaying the broadcast.")]
		public float InitialDelay { get; internal set; }
		[Description("The message to broadcast.")]
		public string Message { get; internal set; }

		public Broadcast(float interval, ushort duration, float initialDelay, string message)
		{
			Interval = interval;
			Duration = duration;
			InitialDelay = initialDelay;
			Message = message;
		}
	}

	public struct Hint
	{
		[Description("Amount of time between the hint being displayed in seconds.")]
		public float Interval { get; internal set; }
		[Description("How long to show the hint in seconds.")]
		public float Duration { get; internal set; }
		[Description("How long in seconds to wait before initially displaying the hint.")]
		public float InitialDelay { get; internal set; }
		[Description("The message to hint.")]
		public string Message { get; internal set; }

		public Hint(float interval, float duration, float initialDelay, string message)
		{
			Interval = interval;
			Duration = duration;
			InitialDelay = initialDelay;
			Message = message;
		}
	}
}
