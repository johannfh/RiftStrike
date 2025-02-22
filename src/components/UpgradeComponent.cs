using System.Collections.Generic;
using Godot;
using Riftstrike.upgrades;

namespace Riftstrike.components {
    [GlobalClass]
    public partial class UpgradeComponent : Node {
        [Signal] public delegate void StatsRecalculatedEventHandler();
        [Export] public StatsComponent StatsComponentSource;
        [Export] public StatsComponent StatsComponentTarget;

        public void Update() {
            StatsComponentTarget.SetStats(StatsComponentSource);
            foreach (var upgrade in Upgrades) {
                upgrade.Apply(StatsComponentTarget);
            }
            EmitSignal(SignalName.StatsRecalculated);
        }

        public List<IUpgrade> Upgrades = new();
    }

    public static class StatsComponentExtensions {
        public static void SetStats(this StatsComponent target, StatsComponent source) {
            target.Health = source.Health;
            target.Regeneration = source.Regeneration;
            target.RiftEnergy = source.RiftEnergy;
        }
    }
}