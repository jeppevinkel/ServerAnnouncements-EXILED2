using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ServerAnnouncements.Api.YamlComments;
using YamlDotNet.Serialization;

namespace ServerAnnouncements.Api
{
	public class Announcements
	{
		public static readonly string PluginPath =
			Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED"), "Plugins"),
				"ServerAnnouncements");
		public static readonly string dataPath = Path.Combine(Path.Combine(PluginPath, Server.Port.ToString()), "data.yml");
		public static readonly string sharedDataPath = Path.Combine(PluginPath, "SharedData.yml");

		[Description("Broadcasts are shown at the top of the screen.")]
		public Dictionary<string, Broadcast> Broadcasts { get; internal set; } = new Dictionary<string, Broadcast>();

		[Description("Hints are shown as smaller text at the bottom of the screen.")]
		public Dictionary<string, Hint> Hints { get; internal set; } = new Dictionary<string, Hint>();

		public void SaveData()
		{
			ISerializer serializer = new SerializerBuilder()
				.WithTypeInspector(inner => new CommentGatheringTypeInspector(inner))
				.WithEmissionPhaseObjectGraphVisitor(args => new CommentsObjectGraphVisitor(args.InnerVisitor))
				.Build();
			string yaml = serializer.Serialize(this);

			Directory.CreateDirectory(Path.Combine(PluginPath, Server.Port.ToString()));

			File.WriteAllText(dataPath, yaml);
		}

		public static void LoadData()
		{
			IDeserializer deserializer =
				new DeserializerBuilder().IgnoreUnmatchedProperties().IgnoreFields().Build();

			if (!File.Exists(dataPath))
			{
				ServerAnnouncements.Announcements = new Announcements();
				ServerAnnouncements.Announcements.Broadcasts = new Dictionary<string, Broadcast>
				{
					{ "playing", new Broadcast(20, 6, 30, $"You are playing on {Server.Name}. Please enjoy your stay!") }
				};
				ServerAnnouncements.Announcements.Hints = new Dictionary<string, Hint>
				{
					{"niceday", new Hint(40, 3, 40, "Have a <color=#10ff10>nice</color> day!")},
					{"enjoy", new Hint(40, 3, 60, "Enjoy your stay!")}
				};

				ServerAnnouncements.Announcements.SaveData();
			}
			else
			{
				string data = File.ReadAllText(dataPath);

				if (string.IsNullOrEmpty(data))
				{
					ServerAnnouncements.Announcements = new Announcements();
					ServerAnnouncements.Announcements.SaveData();

					return;
				}

				var announcements = deserializer.Deserialize<Announcements>(data);

				ServerAnnouncements.Announcements = announcements;
			}

			if (File.Exists(sharedDataPath))
			{
				string sharedData = File.ReadAllText(sharedDataPath);

				if (string.IsNullOrEmpty(sharedData)) return;

				var sharedAnnouncements = deserializer.Deserialize<Announcements>(sharedData);
				ServerAnnouncements.Announcements.Hints.Concat(sharedAnnouncements.Hints)
					.GroupBy(kvp => kvp.Key, kvp => kvp.Value)
					.ToDictionary(g => g.Key, g => g.First());

				ServerAnnouncements.Announcements.Broadcasts.Concat(sharedAnnouncements.Broadcasts)
					.GroupBy(kvp => kvp.Key, kvp => kvp.Value)
					.ToDictionary(g => g.Key, g => g.First());
			}
			else
			{
				ISerializer serializer = new SerializerBuilder()
					.WithTypeInspector(inner => new CommentGatheringTypeInspector(inner))
					.WithEmissionPhaseObjectGraphVisitor(args => new CommentsObjectGraphVisitor(args.InnerVisitor))
					.Build();
				string yaml = serializer.Serialize(new Announcements());

				File.WriteAllText(sharedDataPath, yaml);
			}
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
