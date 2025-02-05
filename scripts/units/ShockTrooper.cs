using Godot;
using Riftstrike.scripts.components;

namespace Riftstrike.scripts.units {
	[GlobalClass]
	public partial class ShockTrooper : Node2D, ICommandable {
		public CommandComponent GetCommandComponent()
			=> GetNode<CommandComponent>("CommandComponent");

		public override void _Ready() {
			GD.Print("ShockTrooper reporting!");
		}
	}
}
