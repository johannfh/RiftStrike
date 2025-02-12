using Riftstrike.components;

namespace Riftstrike.upgrades {
    public interface IUpgrade {
        void Apply(StatsComponent target);
    }
}