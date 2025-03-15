using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.components;
using Riftstrike.enemies;

namespace Riftstrike.src.units
{
    [GlobalClass]
    public abstract partial class Unit : Node2D
    {
        [Signal]
        public delegate void StatsRecalculatedEventHandler();

        [Export]
        public UnitData Data = new();

        public Stats CurrentStats { get; set; } = new();

        [Export(PropertyHint.None, "suffix:pixels")]
        public double SafeDistance = 300;

        public void UpdateStats()
        {
            CurrentStats.SetValuesTo(Data.BaseStats);
            foreach (var upgrade in Data.Upgrades)
            {
                upgrade.Apply(CurrentStats);
            }
            EmitSignal(SignalName.StatsRecalculated);
        }

        public override void _Ready()
        {
            base._Ready();
            UnitManager.Instance.units.Add(this);
            GD.Print($"Units: [{string.Join(", ", UnitManager.Instance.units.Select(u => u.Name))}]");
        }

        public override void _ExitTree()
        {
            UnitManager.Instance.units.Remove(this);
            UnitManager.Instance.unitsSelected.Remove(this);
            base._ExitTree();
        }
    }

    public static class Vector2Extensions
    {
        public static List<Enemy> GetNearbyEnemyChain(this Vector2 position, int count, double range)
        {
            var result = new List<Enemy>();
            for (int i = 0; i < count; i++)
            {
                var enemies = EnemyManager.Enemies
                    .Where(e => !result.Contains(e));

                if (!enemies.Any()) break;

                // get closest enemy
                var closest = enemies
                    .OrderBy((e) => e.GlobalPosition.DistanceTo(position))
                    .First();

                // check if in range of prior position
                var lastPos = i == 0 ? position : result[i - 1].GlobalPosition;
                if (closest.GlobalPosition.DistanceTo(lastPos) > range) break;

                result.Add(closest);
            }
            return result;
        }
    }

    public static class EnemyExtensions
    {
        public static Enemy GetNearestEnemyTo(this Enemy enemy)
        {
            var enemies = EnemyManager.Enemies.Where(e => e != enemy);
            if (!enemies.Any()) return null;

            Enemy nearest = null;
            float distance = float.PositiveInfinity;
            foreach (var other in enemies)
            {
                if (enemy.GlobalPosition.DistanceTo(other.GlobalPosition) < distance)
                {
                    nearest = enemy;
                }
            }
            return nearest;
        }
    }

    public partial class EmptyEnemy : Enemy { }

    public interface IWalk
    {
        public void WalkTo(Vector2 targetPosition, bool append);
    }
}
