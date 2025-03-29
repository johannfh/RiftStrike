using System.Linq;
using Riftstrike.components;
using Riftstrike.src.units;
using Riftstrike.Src.Globals.DamageNumbersDisplay;

namespace Riftstrike.enemies
{
	[GlobalClass]
	public partial class Festerkin : Enemy, IHitable
	{
		private const string SCENE_PATH = "res://src/enemies/festerkin/festerkin.tscn";

		public static PackedScene Scene
			=> GD.Load<PackedScene>(SCENE_PATH);

		public static Festerkin New()
			=> Scene.Instantiate<Festerkin>();

		[Export]
		private NavigationAgent2D NavAgent;

		[Export]
		private AnimationPlayer AnimationPlayer;

		[Export]
		private AudioStreamPlayer2D DeathSoundAudioStreamPlayer;

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
			RecalculateTarget();
			BlinkTimer.Timeout += () =>
			{
				if (AnimationPlayer.CurrentAnimation == "death") return;
				AnimationPlayer.Play("blink");
			};

			AnimationPlayer.AnimationFinished += anim =>
			{
				if (anim == "death")
				{
					QueueFree();
					return;
				}
				AnimationPlayer.Play("walk");
			};

			AttackTimer.WaitTime = attackCooldown;
			HitboxComponent.Hit += (dmg, attacker) =>
			{
				if (attacker.As<GodotObject>() is UnitData unitData)
				{
					Debug.Print(unitData.ResourcePath);
					Hit(dmg, unitData);
				}
			};
			HealthComponent.Death += HandleDeath;
		}

		private UnitData lastAttacker;

		public void Hit(double damage, UnitData attacker)
		{
			lastAttacker = attacker;

			// NOTE: Apply damage modifiers here
			HealthComponent.Damage(damage);
			DamageNumbersDisplay.DisplayNumber(damage, GlobalPosition + new Vector2(0, 50));
		}

		private void HandleDeath()
		{
			// immortal when wave is over
			if (Game.WaveOver) return;

			if (!IsAlive) return;
			IsAlive = false;

			// give last attacker (if any) the experience on death
			if (lastAttacker != null) lastAttacker.Experience += ExperienceReward;

			// play death sound
			DeathSoundAudioStreamPlayer.Reparent(GetParent());
			DeathSoundAudioStreamPlayer.Finished += DeathSoundAudioStreamPlayer.QueueFree;
			DeathSoundAudioStreamPlayer.Play();

			// play death animation
			AnimationPlayer.Play("death");
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
			// freeze when wave is over or died
			if (Game.WaveOver || !IsAlive) return;

			base._PhysicsProcess(delta);
			var nextPos = NavAgent.GetNextPathPosition();
			GlobalPosition = GlobalPosition.MoveToward(nextPos, (float)speed * (float)delta);
			GlobalPosition += PushComponent.PushDirection * (float)pushForce * (float)delta;
			if (!Game.WaveOver && NavAgent.GetFinalPosition().DistanceTo(GlobalPosition) < 5) RecalculateTarget();

			var hitboxes = HitboxComponent.OverlappingHitboxes;
			if (hitboxes.Any() && AttackTimer.IsStopped())
			{
				hitboxes.First().Damage(attackDamage, this);
				AttackTimer.Start();
			}
		}
	}
}
