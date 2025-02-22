using Godot;

namespace Riftstrike.components {
	[GlobalClass]
	public partial class StatsComponent : Node {
		[Export(PropertyHint.None, "0,100,1,or_greater,suffix:hp")]
		public float Health = 50;

        [Export(PropertyHint.None, "0,100,1,or_greater,suffix:per second")]
		public float Regeneration = 0;

        [Export(PropertyHint.None, "0,100,1,or_greater,suffix:RE")]
		public float RiftEnergy = 0;

		[Export(PropertyHint.Range, "0,100,1,or_greater,suffix:%")]
		public float Damage = 100;
		
		
		[Export(PropertyHint.Range, "0,100,1,or_greater,suffix:pixels")]
		public float Range = 500;
	}
}
