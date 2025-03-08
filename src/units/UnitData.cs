using System;
using Godot;
using Godot.Collections;
using Riftstrike.components;
using Riftstrike.upgrades;

namespace Riftstrike.src.units
{
    [GlobalClass]
    public partial class UnitData : Resource
    {
        [Export]
        public UnitType Type;

        [Export]
        public Texture2D Icon;

        [Export]
        public Array<Upgrade> Upgrades = new();

        [Export]
        public Stats BaseStats { get; private set; } = new();

        public ulong Level { get; private set; } = 1;

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

        /// <summary>
        /// A mapping between every existing <see cref="Unit"/> implementation
        /// and the corresponding <see cref="UnitType"/>.
        /// </summary>
        private static readonly System.Collections.Generic.Dictionary<Type, UnitType> UnitTypeMap = new()
        {
            { typeof(RiftAssassin), UnitType.RiftAssassin },
            { typeof(ShockTrooper), UnitType.ShockTrooper },
        };

        public UnitData() { }

        public UnitData(Unit unit)
        {
            if (!UnitTypeMap.TryGetValue(unit.GetType(), out UnitType unitType))
            {
                throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unknown unit type");
            }
            Type = unitType;
        }
    }

    public enum UnitType
    {
        ShockTrooper,
        RiftAssassin,
    }

    public static class UnitTypeExtensions
    {
        public static Unit Instantiate(this UnitType unitType)
        {
            Unit unit = unitType switch
            {
                UnitType.ShockTrooper => UnitFactory.CreateShockTrooper(),
                UnitType.RiftAssassin => UnitFactory.CreateRiftAssassin(),
                _ => throw new ArgumentOutOfRangeException(nameof(unitType), unitType, "Unhandled unit type"),
            };
            return unit;
        }
    }
}