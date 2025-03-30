using System.Linq;
using Godot.Collections;
using Riftstrike.src.units;

namespace Riftstrike.src
{
    public partial class GlobalState : Node
    {
        public const int DEFAULT_WAVE = 1;
        public const int DEFAULT_HIGHEST_DAMAGE = 0;
        public const ulong DEFAULT_RIFT_SHARDS = 0;

        public static GlobalState Instance { get; private set; }

        private readonly Array<UnitData> unitData = [];
        public static Array<UnitData> UnitData
        {
            get => Instance.unitData;
        }

        public static int LevelupsLeft
            => UnitData.Sum(u => u.RemainingLevelups.Count);

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
        private double highestDamage = DEFAULT_HIGHEST_DAMAGE;
        public static double HighestDamage
        {
            get => Instance.highestDamage;
            set => Instance.highestDamage = value;
        }

        public static void Reset()
        {
            Wave = DEFAULT_WAVE;
            RiftShards = DEFAULT_RIFT_SHARDS;
            HighestDamage = DEFAULT_HIGHEST_DAMAGE;
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