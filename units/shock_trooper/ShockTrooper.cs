using Godot;
using Riftstrike.components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Riftstrike.units {
	public partial class ShockTrooper : Unit, IWalk, IAlive {

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

		private HitboxComponent HitboxComponent
			=> GetNode<HitboxComponent>("HitboxComponent");

		private HealthComponent HealthComponent
			=> GetNode<HealthComponent>("HealthComponent");

		private UpgradeComponent UpgradeComponent
			=> GetNode<UpgradeComponent>("UpgradeComponent");
		
		private StatsComponent BaseStatsComponent
			=> GetNode<StatsComponent>("BaseStatsComponent");

		private StatsComponent TargetStatsComponent
			=> GetNode<StatsComponent>("TargetStatsComponent");
		
		private Timer RegenerationTimer
			=> GetNode<Timer>("RegenerationTimer");

		private Sprite2D Sprite
			=> GetNode<Sprite2D>("Sprite2D");

		private Panel SelectedPanel
			=> GetNode<Panel>("SelectedPanel");

		private Panel HoveringPanel
			=> GetNode<Panel>("HoveringPanel");

        public override void _Ready() {
            base._Ready();
			HealthComponent.Health = TargetStatsComponent.Health;
			RegenerationTimer.Timeout += HandleRegen;
			UpgradeComponent.StatsRecalculated += HandleStatsRecalculated;
			UpgradeComponent.Update();
			HitboxComponent.Hit += HandleHit;
			HealthComponent.Death += HandleDeath;
		}

        private void HandleDeath() {
			UnitSelectionManager.Instance.units.Remove(this);
			UnitSelectionManager.Instance.unitsSelected.Remove(this);
			GD.Print($"{Name} died!");
			QueueFree();
        }

        private void HandleRegen() {
			HealthComponent.Health = Mathf.Min(HealthComponent.Health + TargetStatsComponent.Regeneration, TargetStatsComponent.Health);
		}

		private void HandleHit(double damage) {
			// NOTE: Damage absorbtion goes here
			HealthComponent.Damage(damage);
		}

		private void HandleStatsRecalculated() {
			
		}

        public override void _Process(double delta) {
			base._Process(delta);
			SelectedPanel.Visible = SelectableComponent.IsSelected;
			HoveringPanel.Visible = !SelectableComponent.IsSelected && SelectableComponent.IsHovered;
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

		public void WalkTo(Vector2 targetPosition, bool append) {
			if (!append) targets.Clear();
			targets.Add(targetPosition);
		}

        public bool IsAlive() {
			return HealthComponent.Health > 0;
        }
    }
}
