using Godot;
using Riftstrike.components;
using System;
using System.Runtime.CompilerServices;

namespace Riftstrike {
	public partial class ShockTrooper : Unit, IWalk {

		[Export] private float speed = 200;
		[Export] private float pushSpeed = 50;
		private bool targetReached = true;

		private NavigationAgent2D NavAgent
			=> GetNode<NavigationAgent2D>("NavigationAgent2D");
		
		private PushComponent PushComponent
			=> GetNode<PushComponent>("PushComponent");

		public void WalkTo(Vector2 targetPosition) {
			NavAgent.TargetPosition = targetPosition;
			targetReached = false;
		}

		public override void _PhysicsProcess(double delta) {
			base._PhysicsProcess(delta);
			var nextPos = NavAgent.GetNextPathPosition();
			
			if (!targetReached) {
				GlobalPosition = GlobalPosition.MoveToward(nextPos, speed * (float)delta);
				targetReached = NavAgent.IsTargetReached();
			}
			GlobalPosition += PushComponent.PushDirection * pushSpeed * (float)delta;
			
		}
	}
}
