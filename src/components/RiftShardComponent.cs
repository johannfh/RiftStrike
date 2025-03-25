using System.Collections.Generic;
using Riftstrike.src.RiftShard;

namespace Riftstrike.components
{
    [GlobalClass]
    public partial class RiftShardComponent : Area2D
    {
        public override void _Ready()
        {
            base._Ready();
            AreaEntered += OnAreaEntered;
        }

        private readonly List<(RiftShard, float)> collectedShards = [];

        private void OnAreaEntered(Area2D area)
        {
            if (area is RiftShard riftShard && !riftShard.Collected)
            {
                riftShard.Collect();
                collectedShards.Add((riftShard, 0));
            }
        }

        /// <summary>
        /// Acceleration of collected shards in pixels per second
        /// </summary>
        private const double SHARD_ACCELERATION = 1000;

        private const int COLLECT_THRESHOLD = 10;

        public override void _Process(double delta)
        {
            // batch removal
            var shardsToRemove = new List<RiftShard>();

            for (int i = 0; i < collectedShards.Count; i++)
            {
                // deconstruct the tuple
                var (riftShard, speed) = collectedShards[i];

                // accelerate shard
                speed += (float)(SHARD_ACCELERATION * delta);

                // move shard by speed
                riftShard.GlobalPosition = riftShard.GlobalPosition.MoveToward(
                    GlobalPosition, (float)(speed * delta)
                );

                // delete shard once "physically collected"
                if (riftShard.GlobalPosition.DistanceTo(GlobalPosition) < COLLECT_THRESHOLD)
                {
                    riftShard.QueueFree();
                    shardsToRemove.Add(riftShard);
                }

                // update the tuple with the new speed
                collectedShards[i] = (riftShard, speed);
            }

            // remove every "physically collected" shard
            collectedShards.RemoveAll(shard => shardsToRemove.Contains(shard.Item1));
        }
    }
}
