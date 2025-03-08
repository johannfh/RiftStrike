using Godot;
using Riftstrike.src.units;

namespace Riftstrike.upgrades
{
    [GlobalClass]
    public partial class VitalityModule : Upgrade
    {
        [Export]
        public float Regeneration = 3;

        public override void Apply(Stats target)
        {
            target.Regeneration += Regeneration;
        }
    }
}