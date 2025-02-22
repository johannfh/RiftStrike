using Godot;

namespace Riftstrike.components {
    [GlobalClass]
    public partial class HealthBar : ProgressBar {
        [Export] public HealthComponent HealthComponent;

        public override void _Ready() {
            base._Ready();
            MaxValue = HealthComponent.MaxHealth;
            Value = HealthComponent.Health;
        }

        public override void _Process(double delta) {
            base._Process(delta);
            MaxValue = HealthComponent.MaxHealth;
            Value = HealthComponent.Health;
        }
    }
}