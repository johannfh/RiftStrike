using Godot;

namespace Riftstrike.components {
    [GlobalClass]
    public partial class HealthBar : ProgressBar {
        [Export] private HealthComponent HealthComponent;
        [Export] private StatsComponent StatsComponent;

        public override void _Process(double delta) {
            base._Process(delta);
            MaxValue = StatsComponent.Health;
            Value = HealthComponent.Health;
        }
    }
}