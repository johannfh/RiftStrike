using Godot;
using Godot.Collections;
using Riftstrike.upgrades;

namespace Riftstrike.src.units
{
    [GlobalClass]
    public partial class UnitData : Resource
    {
        [Export]
        public PackedScene Scene;

        [Export]
        public Texture2D Icon;

        [Export]
        public Array<Upgrade> Upgrades = new();

        [Export]
        public Stats BaseStats { get; private set; } = new();

        [Export]
        public ulong Level { get; private set; } = 1;

        [Export]
        public Array<ulong> RemainingLevelups = new();

        private double experience;
        public double Experience
        {
            get => experience;
            set
            {
                experience = value;
                var requirements = GetExperienceNeeded(Level + 1);
                if (experience >= requirements)
                {
                    experience -= requirements;
                    Level++;
                    RemainingLevelups.Add(Level);
                }
            }
        }

        public static double GetExperienceNeeded(ulong level)
        {
            return (10 * Mathf.Pow(level, 2)) + (5 * level);
        }

        public Unit InstantiateUnit()
        {
            var unit = Scene.Instantiate<Unit>();
            unit.Data = this;
            return unit;
        }
    }
}