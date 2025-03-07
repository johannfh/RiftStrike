using System;
using Godot;
using Riftstrike.src;
using Riftstrike.src.RiftShard;

namespace Riftstrike.enemies
{
    [GlobalClass]
    public abstract partial class Enemy : Node2D
    {
        [Export(PropertyHint.None, "suffix:xp")]
        public double ExperienceReward = 10;

        [Export(PropertyHint.None)]
        public ulong RiftShardReward = 1;

        static readonly RandomNumberGenerator rng = new();

        public override void _Ready()
        {
            base._Ready();
            rng.Randomize();
            EnemyManager.Instance.enemies.Add(this);
        }

        const float RANDOM_DROP_OFFSET = 50;

        public new void QueueFree()
        {
            // drop rift shard
            var riftShard = GD.Load<PackedScene>("res://src/RiftShard/rift_shard.tscn")
                .Instantiate<RiftShard>();
            riftShard.GlobalPosition = GlobalPosition + new Vector2(
                rng.RandfRange(-RANDOM_DROP_OFFSET, RANDOM_DROP_OFFSET),
                rng.RandfRange(-RANDOM_DROP_OFFSET, RANDOM_DROP_OFFSET)
            );
            riftShard.Reward = RiftShardReward;
            AddSibling(riftShard);

            base.QueueFree();
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            EnemyManager.Instance.enemies.Remove(this);
        }
    }
}