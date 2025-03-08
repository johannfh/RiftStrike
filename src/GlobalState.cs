using Godot;
using Godot.Collections;
using Riftstrike.src.units;

namespace Riftstrike.src
{
    public partial class GlobalState : Node
    {
        private const int DEFAULT_WAVE = 1;
        private const ulong DEFAULT_RIFT_SHARDS = 0;

        public static GlobalState Instance { get; private set; }

        private readonly Array<UnitData> unitData = new();
        public static Array<UnitData> UnitData
        {
            get => Instance.unitData;
        }

        [Export]
        private ulong riftShards = DEFAULT_RIFT_SHARDS;
        public static ulong RiftShards
        {
            get => Instance.riftShards;
            set => Instance.riftShards = value;
        }

        private int wave = DEFAULT_WAVE;
        public static int Wave
        {
            get => Instance.wave;
            set => Instance.wave = value;
        }

        public static void Reset()
        {
            Wave = DEFAULT_WAVE;
            RiftShards = DEFAULT_RIFT_SHARDS;
            UnitData.Clear();
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