using System.Collections.Generic;
using Godot;

namespace Riftstrike.scripts.utils {
	public partial class ScreenLoader : Node {
		public static ScreenLoader Instance { get; private set; }
		private static readonly string screenDir = "res://scenes/screens";
		private static readonly Dictionary<string, PackedScene> screens = new();

		// WARNING: this load all scenes upfront, shifting load times to the
		// initial game start. This could add unwanted overhead; *monitor*
        public override void _Ready() {
            Instance = this;
			foreach (var sceneName in DirAccess.GetFilesAt($"{screenDir}/")) {
				var scene = GD.Load<PackedScene>($"{screenDir}/{sceneName}");
				screens[sceneName] = scene;
				GD.Print($"Loaded screen \"{sceneName}\" at \"{screenDir}/{sceneName}\"");
			};
        }

        public static void SetScreen(string screenName) {
			if (!screens.ContainsKey(screenName)) {
				GD.PushError($"Screen \"{screenName}\" not found in preloaded {nameof(screens)}.");
			} else {
				var scene = screens[screenName];
				Instance.GetTree().ChangeSceneToPacked(scene);
			}
        }
    }
}
