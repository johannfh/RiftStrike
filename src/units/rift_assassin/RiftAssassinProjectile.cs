using System.Collections.Generic;
using System.Linq;
using Riftstrike.enemies;

namespace Riftstrike.src.units
{
    public partial class RiftAssassinProjectile : Node2D
    {
        private const string SCENE_PATH = "res://src/units/rift_assassin/rift_assassin_projectile.tscn";

        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);

        public static RiftAssassinProjectile New()
            => Scene.Instantiate<RiftAssassinProjectile>();

        [Export]
        private double Speed;

        [Export]
        private Sprite2D Sprite;

        [Export]
        private bool Debug;

        public UnitData UnitData;

        public List<Enemy> Enemies;

        public const float ATTACK_HITBOX = 10;

        public double Damage;

        public override void _PhysicsProcess(double delta)
        {
            if (Game.WaveOver)
            {
                QueueFree();
                return;
            }

            base._PhysicsProcess(delta);

            // remove any invalid instances (e.g. if an enemy has died recently)
            Enemies.RemoveAll(enemy => !IsInstanceValid(enemy));

            if (Enemies.Count == 0)
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

        public override void _Process(double delta)
        {
            base._Process(delta);
            UpdateSpriteRotation();
            QueueRedraw();
        }

        private void UpdateSpriteRotation()
        {
            if (Enemies.Count == 0) return;
            var nextTarget = Enemies.First();
            if (!IsInstanceValid(nextTarget)) return;
            Sprite.Rotation = GlobalPosition.AngleToPoint(nextTarget.GlobalPosition);
        }


        public override void _Draw()
        {
            base._Draw();
            if (Debug) DrawDebugTargetLine();
        }

        private void DrawDebugTargetLine()
        {
            if (Enemies.Count == 0) return;
            var firstEnemy = Enemies[0];
            if (IsInstanceValid(firstEnemy))
                DrawLine(Vector2.Zero, Enemies[0].GlobalPosition - GlobalPosition, Colors.Red);

            if (Enemies.Count < 2) return;
            for (int i = 1; i < Enemies.Count; i++)
            {
                var fromEnemy = Enemies[i - 1];
                var toEnemy = Enemies[i];

                if (!IsInstanceValid(fromEnemy) || !IsInstanceValid(toEnemy)) continue;

                var from = fromEnemy.GlobalPosition - GlobalPosition;
                var to = toEnemy.GlobalPosition - GlobalPosition;
                DrawLine(from, to, Colors.Red);
            }
        }
    }
}
