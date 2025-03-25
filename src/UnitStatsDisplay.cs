using Riftstrike.src.units;

namespace Riftstrike
{
    public partial class UnitStatsDisplay : MarginContainer
    {
        #region Node References
        [Export]
        private Label HealthStatLabel;

        [Export]
        private Label RegenerationStatLabel;

        [Export]
        private Label DamageStatLabel;
        #endregion

#nullable enable

        public Stats? Stats;

        public override void _Process(double delta)
        {
            base._Process(delta);

            // refresh ui
            if (Stats != null)
            {
                HealthStatLabel.Text = Stats.Health.ToString();
                RegenerationStatLabel.Text = Stats.Regeneration.ToString();
                DamageStatLabel.Text = $"{Stats.Damage}%";
            }
        }
    }
}
