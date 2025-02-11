using Godot;

namespace Riftstrike.components {
    public partial class HitboxComponent : Area2D {
        [Signal] public delegate void HitEventHandler(double damage);

        public void Damage(double damage) {
            EmitSignal(SignalName.Hit, damage);
        }
    }
}
