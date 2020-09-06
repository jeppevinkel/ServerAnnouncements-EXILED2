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

        private static readonly List<CoroutineHandle> _coroutines = new List<CoroutineHandle>();

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

	        Timing.KillCoroutines(_coroutines);
	        _coroutines.Clear();
        }

        public static void ActivateAnnouncements()
        {
	        foreach (Api.Broadcast broadcast in Announcements.Broadcasts)
	        {
		        _coroutines.Add(Timing.RunCoroutine(PlayBroadcast(broadcast)));
		        Log.Debug($"Loaded broadcast: {broadcast.Message}", Loader.ShouldDebugBeShown);
			}

	        foreach (Hint hint in Announcements.Hints)
	        {
		        _coroutines.Add(Timing.RunCoroutine(PlayHint(hint)));
				Log.Debug($"Loaded hint: {hint.Message}", Loader.ShouldDebugBeShown);
	        }

			Log.Debug($"Total running coroutines: {_coroutines.Count}", Loader.ShouldDebugBeShown);
        }

        public static void ReloadAnnouncements()
        {
	        Timing.KillCoroutines(_coroutines);

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
			        player.ShowHint($"\n\n\n\n\n{hint.Message}", hint.Duration);
		        }
		        yield return Timing.WaitForSeconds(hint.Interval);
	        }
        }
    }
}
