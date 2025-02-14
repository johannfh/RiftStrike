using System.Linq;
using Godot;
using Riftstrike.components;

namespace Riftstrike.enemies {
	public partial class Festerkin : Node2D {
		[ExportGroup("Movement")]
		[Export] private double speed = 200;
		[Export] private double pushForce = 100;
		[Export] private double targetOvershootDistance = 100;
		[ExportGroup("Attacks")]
		[Export] public double attackDamage = 10;
		[Export(PropertyHint.None, "suffix:s")] public double attackCooldown = 1;

		private NavigationAgent2D NavAgent
			=> GetNode<NavigationAgent2D>("NavigationAgent2D");

		private AnimationPlayer AnimationPlayer
			=> GetNode<AnimationPlayer>("AnimationPlayer");

		private Sprite2D Sprite
			=> GetNode<Sprite2D>("Sprite2D");
		
		private Timer RecalculateTargetTimer
			=> GetNode<Timer>("RecalculateTargetTimer");

		private PushComponent PushComponent
			=> GetNode<PushComponent>("PushComponent");
		
		private HitboxComponent HitboxComponent
			=> GetNode<HitboxComponent>("HitboxComponent");
		
		private Timer AttackTimer
			=> GetNode<Timer>("AttackTimer");
		
		private RandomTimer BlinkTimer
			=> GetNode<RandomTimer>("BlinkTimer");

		public override void _Ready() {
			base._Ready();
			AnimationPlayer.Play("walk");
			RecalculateTargetTimer.Timeout += RecalculateTarget;
			BlinkTimer.Timeout += () => AnimationPlayer.Play("blink");
			AnimationPlayer.AnimationFinished += (_) => AnimationPlayer.Play("walk");
			AttackTimer.WaitTime = attackCooldown;
		}

		private void RecalculateTarget() {
			// TODO: implement calculation (first errors when none found; make it based on area and detection or closest player unit global?)
			var units = UnitSelectionManager.Instance.units
				.OrderBy(u => GlobalPosition.DistanceTo(u.GlobalPosition));
			
			if (units.Any()) {
				var targetPos = units.First().GlobalPosition;
				NavAgent.TargetPosition = targetPos + (GlobalPosition.DirectionTo(targetPos) * (float)targetOvershootDistance);
			}
		}

		public override void _PhysicsProcess(double delta) {
			base._PhysicsProcess(delta);
			var nextPos = NavAgent.GetNextPathPosition();
			GlobalPosition = GlobalPosition.MoveToward(nextPos, (float)speed * (float)delta);
			GlobalPosition += PushComponent.PushDirection * (float)pushForce * (float)delta;
			if (NavAgent.GetFinalPosition().DistanceTo(GlobalPosition) < 5) RecalculateTarget();

			var hitboxes = HitboxComponent.OverlappingHitboxes;
			if (hitboxes.Any() && AttackTimer.IsStopped()) {
				hitboxes.First().Damage(attackDamage);
				AttackTimer.Start();
				GD.Print($"attacking {hitboxes.First().GetParent().Name}");
			}
		}
	}
}
