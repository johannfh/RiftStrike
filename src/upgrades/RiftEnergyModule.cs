// Some Node
using Godot;
using Riftstrike.components;

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