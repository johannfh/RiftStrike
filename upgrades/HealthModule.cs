using Godot;
using Riftstrike.components;

namespace Riftstrike.upgrades {
    /// <summary>
    /// Increases max health by 10. 
    /// </summary>
    public partial class HealthModule : IUpgrade {
        public void Apply(StatsComponent target) {
            target.Health += 10;
        }

        public Texture2D GetIcon() {
            throw new System.NotImplementedException();
        }
    }
}