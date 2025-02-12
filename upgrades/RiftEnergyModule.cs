using Godot;
using Riftstrike.components;

namespace Riftstrike.upgrades {
    public class RiftEnergyModule : IUpgrade {
        public void Apply(StatsComponent target) {
            target.Damage += 5;
        }

        public Texture2D GetIcon() {
            throw new System.NotImplementedException();
        }
    }
}