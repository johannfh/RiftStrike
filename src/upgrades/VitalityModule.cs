using Godot;
using Riftstrike.components;

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