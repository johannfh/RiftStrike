using Godot;

namespace Riftstrike.components {
	public partial class StatsComponent : Node {
		[Export] public float Health;
        [Export] public float Regeneration;
        [Export] public float RiftEnergy;
	}
}
