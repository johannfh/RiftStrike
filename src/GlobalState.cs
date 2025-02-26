using Godot;
using Godot.Collections;
using Riftstrike.src.units;

namespace Riftstrike.src
{
    public partial class GlobalState : Node
    {
        public static GlobalState Instance { get; private set; }

        public readonly Array<UnitData> UnitData = new();

        [Export]
        private int enemySpawnCounter = 0;
        public static int EnemySpawnCounter
        {
            get => Instance.enemySpawnCounter;
            set => Instance.enemySpawnCounter = value;
        }

        private int wave = 1;
        public static int Wave
        {
            get => Instance.wave;
            set => Instance.wave = value;
        }

        public static void IncrementEnemySpawnCounter()
        {
            EnemySpawnCounter++;
            GD.Print($"Total enemies spawned: {EnemySpawnCounter}!");
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