using Godot;

namespace Riftstrike {
    [Tool]
	[GlobalClass]
	public partial class RandomTimer : Timer {
		[ExportGroup("Random Delay")]
		[Export(PropertyHint.Range, "0,60,0.1,or_greater,suffix:s")] public float minimumDelay;
		[Export(PropertyHint.Range, "0,60,0.1,or_greater,suffix:s")] public float maximumDelay;

		private RandomNumberGenerator rng = new();

		public override void _Ready() {
			rng.Randomize();
			
			if (Autostart && !Engine.IsEditorHint()) {
                GD.Print($"Starting {Name}: min({minimumDelay}) max({maximumDelay})");
				StartRandom();
			}
		}

		public void StartRandom() {
            var rand = rng.RandfRange(minimumDelay, maximumDelay);
			Start(rand);
		}

		private void OnTimeout() {
			if (!OneShot) {
				StartRandom();
			}
		}
	}
}
