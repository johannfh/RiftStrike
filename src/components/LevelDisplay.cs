using Godot;
using Riftstrike.src.units;
using System;

namespace Riftstrike.components
{
    public partial class LevelDisplay : Control
    {
        [Export]
        private Unit unit;

        private Label label;
        private ProgressBar progressBar;

        public override void _Ready()
        {
            base._Ready();
            label = GetNode<Label>("Label");
            progressBar = GetNode<ProgressBar>("ProgressBar");
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            label.Text = $"{unit.Data.Level}";
            progressBar.MaxValue = UnitData.GetExperienceNeeded(unit.Data.Level + 1);
            progressBar.Value = unit.Data.Experience;
        }
    }
}
