using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Riftstrike.components {
    public partial class HitboxComponent : Area2D {
        [Signal] public delegate void HitEventHandler(double damage);

        public void Damage(double damage) {
            EmitSignal(SignalName.Hit, damage);
        }
        public IEnumerable<HitboxComponent> OverlappingHitboxes
            => GetOverlappingAreas()
                .OfType<HitboxComponent>()
                .Where(hitbox => hitbox != this);
    }
}
