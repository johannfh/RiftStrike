using System.Linq;
using Godot;
using Riftstrike.components;

namespace Riftstrike.src.units
{
	public partial class ShockTrooperProjectile : Area2D
	{
		public static ShockTrooperProjectile New()
		{
			return GD.Load<PackedScene>(
				"res://src/units/shock_trooper/shock_trooper_projectile.tscn"
				)
				.Instantiate() as ShockTrooperProjectile;
		}

		public Vector2 Velocity = Vector2.Zero;

		public double Damage;
		public double Range;

		public UnitData UnitData;

		private HitboxComponent lastCollision = null;
		private AnimatedSprite2D AnimatedSprite
			=> GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		private int remainingHits;
		public int RemainingHits
		{
			get => remainingHits;
			set
			{
				remainingHits = value;
				if (remainingHits <= 0)
				{
					QueueFree();
				}
			}
		}

		private double distanceTravelled;
		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			GlobalPosition += Velocity * (float)delta;
			distanceTravelled += Velocity.Length() * (float)delta;
			var overlappingHitboxes = GetOverlappingAreas()
				.OfType<HitboxComponent>()
				.Where(h => h != lastCollision)
				.OrderBy(h => h.GlobalPosition.DistanceTo(GlobalPosition));

			if (overlappingHitboxes.Any())
			{
				var target = overlappingHitboxes.First();
				target.Damage(Damage, UnitData);
				lastCollision = target;
				distanceTravelled = 0;
				RemainingHits--;
			}
			if (distanceTravelled > Range)
			{
				QueueFree();
			}
		}

		private void UpdateRotation()
		{
			Rotation = Velocity.Normalized().Angle();
		}

		public override void _Ready()
		{
			base._Ready();
			UpdateRotation();
			AnimatedSprite.Play();
		}
	}
}
