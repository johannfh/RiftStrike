using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.enemies;

namespace Riftstrike.units
{
    [GlobalClass]
    public abstract partial class Unit : Node2D
    {
        [Export(PropertyHint.None, "suffix:pixels")]
        public double SafeDistance = 300;

        public override void _Ready()
        {
            base._Ready();
            UnitManager.Instance.units.Add(this);
            GD.Print(UnitManager.Instance.Name);
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
            var result = new List<Enemy>() {
                new EmptyEnemy {
                    GlobalPosition = position,
                },
            };
            for (int i = 1; i < count + 1; i++)
            {
                var prior = result.ElementAt(i - 1);
                var nearest = prior.GetNearestEnemyTo();
                if (nearest == null
                    || prior.GlobalPosition.DistanceTo(nearest.GlobalPosition) > range)
                {
                    break;
                }
                result.Add(nearest);
            }
            result.RemoveAt(0);
            return result;
        }
    }

    public static class EnemyExtensions
    {
        public static Enemy GetNearestEnemyTo(this Enemy enemy)
        {
            var enemies = EnemyManager.Instance.enemies.Where(e => e != enemy);
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
