using Godot;
using Riftstrike.src;

namespace Riftstrike.enemies
{
    [GlobalClass]
    public abstract partial class Enemy : Node2D
    {
        [Export(PropertyHint.None, "suffix:xp")]
        public double ExperienceReward = 10;

        [Export(PropertyHint.None, "suffix:rs")]
        public double RiftShardReward = 1;

        public override void _Ready()
        {
            base._Ready();
            EnemyManager.Instance.enemies.Add(this);
        }

        public new void QueueFree()
        {
            var experience = GD.Load<PackedScene>("res://src/experience.tscn")
                .Instantiate<Experience>();
            experience.Value = ExperienceReward;
            experience.GlobalPosition = GlobalPosition;
            AddSibling(experience);
            GlobalState.RiftShards += RiftShardReward;
            base.QueueFree();
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            EnemyManager.Instance.enemies.Remove(this);
        }
    }
}