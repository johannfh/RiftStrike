using System.Collections.Generic;
using System.Linq;

namespace Riftstrike.components
{
    [GlobalClass]
    public partial class HitboxComponent : Area2D
    {
        [Signal] public delegate void HitEventHandler(double damage, Variant attacker);

        [Export]
        public Node2D Unit;

        public void Damage(double damage, Variant attacker)
        {
            EmitSignal(SignalName.Hit, damage, attacker);
        }

        public IEnumerable<HitboxComponent> OverlappingHitboxes
            => GetOverlappingAreas()
                .OfType<HitboxComponent>()
                .Where(hitbox => hitbox != this);
    }
}
