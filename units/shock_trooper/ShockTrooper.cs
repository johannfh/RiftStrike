using Godot;
using Riftstrike.components;
using System.Collections.Generic;
using System.Linq;

namespace Riftstrike.units {
	public partial class ShockTrooper : Unit, IWalk {

		[Export] private float speed = 200;
		[Export] private float pushSpeed = 50;
		private bool AllTargetsCleared => !targets.Any();
		private readonly List<Vector2> targets = new();

        private NavigationAgent2D NavAgent
			=> GetNode<NavigationAgent2D>("NavigationAgent2D");

		private PushComponent PushComponent
			=> GetNode<PushComponent>("PushComponent");
		
		private SelectableComponent SelectableComponent
			=> GetNode<SelectableComponent>("SelectableComponent");

		private Sprite2D Sprite
			=> GetNode<Sprite2D>("Sprite2D");

		private Panel SelectedPanel
			=> GetNode<Panel>("SelectedPanel");

		private Panel HoveringPanel
			=> GetNode<Panel>("HoveringPanel");

		public override void _Process(double delta) {
			base._Process(delta);
			SelectedPanel.Visible = SelectableComponent.IsSelected;
			HoveringPanel.Visible = !SelectableComponent.IsSelected && SelectableComponent.IsHovered;
		}

		public void WalkTo(Vector2 targetPosition, bool append) {
			if (!append) targets.Clear();
			targets.Add(targetPosition);
		}

		public override void _PhysicsProcess(double delta) {
			base._PhysicsProcess(delta);
			if (targets.Any() && NavAgent.TargetPosition != targets.First()) {
				NavAgent.TargetPosition = targets.First();
			}
			if (targets.Any() && NavAgent.GetFinalPosition().DistanceTo(GlobalPosition) < 5) targets.RemoveAt(0);
			var nextPos = NavAgent.GetNextPathPosition();

			if (!AllTargetsCleared) {
				Sprite.FlipH = GlobalPosition.DirectionTo(NavAgent.TargetPosition).X < 0;
				GlobalPosition = GlobalPosition.MoveToward(nextPos, speed * (float)delta);
			}
			GlobalPosition += PushComponent.PushDirection * pushSpeed * (float)delta;
		}
	}
}
