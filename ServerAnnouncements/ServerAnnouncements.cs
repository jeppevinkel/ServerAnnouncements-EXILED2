using System.Collections;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Loader;
using MEC;
using ServerAnnouncements.Api;

namespace ServerAnnouncements
{
    public class ServerAnnouncements : Plugin<Config>
    {
        public static ServerAnnouncements Instance { get; } = new ServerAnnouncements();
        private ServerAnnouncements() { }

        public static Announcements Announcements;

        private static readonly List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        public override void OnEnabled()
        {
	        base.OnEnabled();

            Announcements.LoadData();
			
            ActivateAnnouncements();

			Log.Info("Announcements have been loaded.");
            Log.Info($"Announcements can be found and modified in {Announcements.PluginPath}");
        }

        public override void OnDisabled()
        {
	        base.OnDisabled();

	        Timing.KillCoroutines(Coroutines);
	        Coroutines.Clear();
        }

        public static void ActivateAnnouncements()
        {
	        foreach (KeyValuePair<string, Api.Broadcast> broadcast in Announcements.Broadcasts)
	        {
		        Coroutines.Add(Timing.RunCoroutine(PlayBroadcast(broadcast.Value)));
		        Log.Debug($"Loaded broadcast: {broadcast.Key}", Loader.ShouldDebugBeShown);
			}

	        foreach (KeyValuePair<string, Hint> hint in Announcements.Hints)
	        {
		        Coroutines.Add(Timing.RunCoroutine(PlayHint(hint.Value)));
				Log.Debug($"Loaded hint: {hint.Key}", Loader.ShouldDebugBeShown);
	        }

			Log.Debug($"Total running coroutines: {Coroutines.Count}", Loader.ShouldDebugBeShown);
        }

        public static void ReloadAnnouncements()
        {
	        Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();

			Announcements.LoadData();

			ActivateAnnouncements();
        }

        private static IEnumerator<float> PlayBroadcast(Api.Broadcast broadcast)
        {
	        yield return Timing.WaitForSeconds(broadcast.InitialDelay);

	        while (true)
	        {
		        Map.Broadcast(broadcast.Duration, broadcast.Message);
		        yield return Timing.WaitForSeconds(broadcast.Interval);
	        }
		}

        private static IEnumerator<float> PlayHint(Hint hint)
        {
	        yield return Timing.WaitForSeconds(hint.InitialDelay);

	        while (true)
	        {
		        foreach (Player player in Player.List)
		        {
			        player.ShowHint($"\n\n\n\n\n\n\n{hint.Message}", hint.Duration);
		        }
		        yield return Timing.WaitForSeconds(hint.Interval);
	        }
        }
    }
}
