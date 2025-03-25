// Some Node
using Riftstrike.src.units;

namespace Riftstrike.upgrades
{
    [GlobalClass]
    public partial class RiftEnergyModule : Upgrade
    {
        [Export] float RiftEnergy = 5;
        public override void Apply(Stats target)
        {
            target.RiftEnergy += RiftEnergy;
        }
    }
}