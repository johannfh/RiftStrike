using Godot;

namespace Riftstrike.enemies {
    [GlobalClass]
    public abstract partial class Enemy : Node2D {
        public override void _Ready() {
            base._Ready();
            EnemyManager.Instance.enemies.Add(this);
        }

        public override void _ExitTree() {
            base._ExitTree();
            EnemyManager.Instance.enemies.Remove(this);
        }
    }
}