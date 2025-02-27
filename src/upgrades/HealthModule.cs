using Godot;
using Riftstrike.components;

namespace Riftstrike.upgrades
{
    [GlobalClass]
    public partial class HealthModule : Upgrade
    {
        [Export]
        public float Health = 10;
        public override void Apply(Stats target)
        {
            target.Health += Health;
        }
    }
}
