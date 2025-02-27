using Godot;
using Godot.Collections;
using Riftstrike.src.units;

namespace Riftstrike.src
{
    public partial class GlobalState : Node
    {
        public static GlobalState Instance { get; private set; }

        private readonly Array<UnitData> unitData = new();
        public static Array<UnitData> UnitData
        {
            get => Instance.unitData;
        }

        [Export]
        private double riftShards = 0;
        public static double RiftShards
        {
            get => Instance.riftShards;
            set => Instance.riftShards = value;
        }

        private int wave = 1;
        public static int Wave
        {
            get => Instance.wave;
            set => Instance.wave = value;
        }

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
        }
    }
}