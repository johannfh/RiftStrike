using System.Linq;
using Godot;
using Riftstrike.components;

namespace Riftstrike.src.units
{
    public partial class ShockTrooperProjectile : Area2D
    {
        public const string SCENE_PATH = "res://src/units/shock_trooper/shock_trooper_projectile.tscn";

        public static ShockTrooperProjectile New()
            => GD.Load<PackedScene>(SCENE_PATH)
                .Instantiate<ShockTrooperProjectile>();

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

                // get enemies ordered by distance
                // var enemies = EnemyManager.Enemies
                //     .OrderBy(e => e.GlobalPosition.DistanceTo(GlobalPosition));

                // retarget closest next enemy
                // if (enemies.Any())
                // {
                //     Debug.Print("Retargeting");
                //     var closestEnemy = enemies.First();
                //     if (closestEnemy.GlobalPosition.DistanceTo(GlobalPosition) < Range)
                //     {
                //         var speed = Velocity.Length();
                //         Velocity = GlobalPosition.DirectionTo(closestEnemy.GlobalPosition) * speed;
                //     }
                // }
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
