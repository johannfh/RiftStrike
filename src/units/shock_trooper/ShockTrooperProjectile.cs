using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.components;
using Riftstrike.enemies;

namespace Riftstrike.src.units
{
    public partial class ShockTrooperProjectile : Area2D
    {
        private const string SCENE_PATH = "res://src/units/shock_trooper/shock_trooper_projectile.tscn";

        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);

        public static ShockTrooperProjectile New()
            => Scene.Instantiate<ShockTrooperProjectile>();

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

        private readonly List<HitboxComponent> alreadyHit = [];

        private double distanceTravelled;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            // move bullet
            GlobalPosition += Velocity * (float)delta;

            // count distance travelled since last hit
            distanceTravelled += Velocity.Length() * (float)delta;

            // filter dead enemies
            alreadyHit.RemoveAll(h => !IsInstanceValid(h));

            // get collisions
            var overlappingHitboxes = GetOverlappingAreas()
                .OfType<HitboxComponent>()
                .Where(h => !alreadyHit.Contains(h))
                .OrderBy(h => h.GlobalPosition.DistanceTo(GlobalPosition));

            // check if there are any collisions
            if (overlappingHitboxes.Any())
            {
                var target = overlappingHitboxes.First();
                target.Damage(Damage, UnitData);
                alreadyHit.Add(target);
                distanceTravelled = 0;
                RemainingHits--;

                var alreadyHitEnemies = alreadyHit.Select(h => h.Owner as Enemy);

                // get all (not yet hit) enemies ordered by distance
                var enemies = EnemyManager.Enemies
                    .OrderBy(e => e.GlobalPosition.DistanceTo(GlobalPosition))
                    .Where(e => !alreadyHitEnemies.Contains(e));

                // retarget closest next enemy (if any)
                if (enemies.Any())
                {
                    Debug.Print("Retargeting");

                    var closestEnemy = enemies.First();
                    if (closestEnemy.GlobalPosition.DistanceTo(GlobalPosition) < Range)
                    {
                        var speed = Velocity.Length();
                        Velocity = GlobalPosition.DirectionTo(closestEnemy.GlobalPosition) * speed;
                    }
                    // closest is out of range
                    else QueueFree();
                }
                // no more enemies left
                else QueueFree();
            }

            // max travel distance reached
            if (distanceTravelled > Range) QueueFree();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            UpdateRotation();
        }

        private void UpdateRotation()
            => Rotation = Velocity.Normalized().Angle();

        public override void _Ready()
        {
            base._Ready();
            AnimatedSprite.Play();
        }
    }
}
