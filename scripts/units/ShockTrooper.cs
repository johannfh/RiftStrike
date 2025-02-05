using Godot;
using System;

namespace Units {
	public partial class ShockTrooper : Node2D {
		public override void _Ready() {
			GD.Print("ShockTrooper reporting!");
		}
	}
}
