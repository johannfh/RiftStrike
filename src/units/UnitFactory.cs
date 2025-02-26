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
}
