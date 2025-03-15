

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;
using Riftstrike.enemies;

namespace Riftstrike.src.units
{
    public partial class RiftAssassinProjectile : Node2D
    {
        public const string SCENE_PATH = "res://src/units/rift_assassin/rift_assassin_projectile.tscn";

        public static RiftAssassinProjectile New()
            => GD.Load<PackedScene>(SCENE_PATH)
                .Instantiate<RiftAssassinProjectile>();

        [Export]
        private double Speed;

        public UnitData UnitData;

        public List<Enemy> Enemies;

        public const float ATTACK_HITBOX = 10;

        public double Damage;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            // remove any invalid instances (e.g. if an enemy has died recently)
            Enemies.RemoveAll(enemy => !IsInstanceValid(enemy));

            if (!Enemies.Any())
            {
                QueueFree();
                return;
            }

            var first = Enemies.First();

            GlobalPosition = GlobalPosition.MoveToward(first.GlobalPosition, (float)(Speed * delta));

            if (GlobalPosition.DistanceTo(first.GlobalPosition) <= ATTACK_HITBOX)
            {
                // apply damage if hitable
                if (first is IHitable hitable) hitable.Hit(Damage, UnitData);
                Enemies.RemoveAt(0);
            }
        }
    }
}