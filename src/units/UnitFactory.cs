using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Riftstrike.upgrades;

namespace Riftstrike.src.units
{
    public partial class UnitFactory : Node
    {
        private static UnitFactory Instance;

        private PackedScene shockTrooperScene;
        private PackedScene riftAssassinScene;

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;

            shockTrooperScene = GD.Load<PackedScene>($"res://src/units/shock_trooper/shock_trooper.tscn");
            riftAssassinScene = GD.Load<PackedScene>($"res://src/units/rift_assassin/rift_assassin.tscn");
        }

        public static ShockTrooper CreateShockTrooper()
            => Instance.shockTrooperScene.Instantiate<ShockTrooper>();

        public static RiftAssassin CreateRiftAssassin()
            => Instance.riftAssassinScene.Instantiate<RiftAssassin>();

        static UnitFactory()
        {
            GD.Print($"Initialized {nameof(UnitFactory)}");
        }
    }

    public partial class UnitData : Resource
    {
        public UnitType Type;
        public Array<Upgrade> Upgrades = new();

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
            Upgrades = unit.Upgrades;
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
