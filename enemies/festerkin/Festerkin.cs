using System.Linq;
using Godot;
using Riftstrike.components;

namespace Riftstrike.enemies
{
	[GlobalClass]
	public partial class Festerkin : Enemy
	{
		[Export]
		private NavigationAgent2D NavAgent;

		[Export]
		private AnimationPlayer AnimationPlayer;

		[Export]
		private Sprite2D Sprite;

		[Export]
		private Timer RecalculateTargetTimer;

		[Export]
		private PushComponent PushComponent;

		[Export]
		private HitboxComponent HitboxComponent;

		[Export]
		private Timer AttackTimer;

		[Export]
		private RandomTimer BlinkTimer;

		[Export]
		private HealthComponent HealthComponent;
		[ExportGroup("Movement")]
		[Export] private double speed = 200;
		[Export] private double pushForce = 100;
		[Export] private double targetOvershootDistance = 100;

		[ExportGroup("Attacks")]
		[Export]
		public double attackDamage = 10;

		[Export(PropertyHint.None, "suffix:s")]
		public double attackCooldown = 1;

		public override void _Ready()
		{
			base._Ready();
			AnimationPlayer.Play("walk");
			RecalculateTargetTimer.Timeout += RecalculateTarget;
			BlinkTimer.Timeout += () => AnimationPlayer.Play("blink");
			AnimationPlayer.AnimationFinished += (_) => AnimationPlayer.Play("walk");
			AttackTimer.WaitTime = attackCooldown;
			HitboxComponent.Hit += HandleHit;
			HealthComponent.Death += HandleDeath;
		}

		private void HandleHit(double damage)
		{
			// NOTE: Apply damage modifiers here
			HealthComponent.Damage(damage);
		}

		private void HandleDeath()
		{
			QueueFree();
		}

		private void RecalculateTarget()
		{
			// TODO: implement calculation (first errors when none found; make it based on area and detection or closest player unit global?)
			var units = UnitManager.Instance.units
				.OrderBy(u => GlobalPosition.DistanceTo(u.GlobalPosition));

			if (units.Any())
			{
				var targetPos = units.First().GlobalPosition;
				NavAgent.TargetPosition = targetPos + (GlobalPosition.DirectionTo(targetPos) * (float)targetOvershootDistance);
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			var nextPos = NavAgent.GetNextPathPosition();
			GlobalPosition = GlobalPosition.MoveToward(nextPos, (float)speed * (float)delta);
			GlobalPosition += PushComponent.PushDirection * (float)pushForce * (float)delta;
			if (NavAgent.GetFinalPosition().DistanceTo(GlobalPosition) < 5) RecalculateTarget();

			var hitboxes = HitboxComponent.OverlappingHitboxes;
			if (hitboxes.Any() && AttackTimer.IsStopped())
			{
				hitboxes.First().Damage(attackDamage);
				AttackTimer.Start();
			}
		}
	}
}
