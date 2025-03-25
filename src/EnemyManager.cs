using System.Collections.Generic;
using Godot;
using Riftstrike.enemies;

namespace Riftstrike
{
    [GlobalClass]
    public partial class EnemyManager : Node2D
    {
        public static EnemyManager Instance { get; private set; }
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

        public override void _ExitTree()
        {
            base._ExitTree();
            if (Instance == this) Instance = null;
        }

        private readonly List<Enemy> enemies = [];
        public static List<Enemy> Enemies => Instance.enemies;
    }
}