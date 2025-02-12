using Riftstrike.components;

namespace Riftstrike.upgrades {
    /// <summary>
    /// Increases max health by 10. 
    /// </summary>
    public partial class HealthModule : IUpgrade {
        public void Apply(StatsComponent target) {
            target.Health += 10;
        }
    }
}