using Godot;
using Riftstrike.components;

namespace Riftstrike.upgrades
{
    [GlobalClass]
    public partial class DamageModule : Upgrade
    {
        [Export]
        float Damage = 5;

        public override void Apply(Stats target)
        {
            target.Damage += Damage;
        }
    }
}