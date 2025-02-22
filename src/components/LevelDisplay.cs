using Godot;
using System;

namespace Riftstrike.components
{
    public partial class LevelDisplay : Control
    {
        [Export]
        private LevelComponent levelComponent;

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
            label.Text = $"{levelComponent.Level}";
            progressBar.MaxValue = LevelComponent.GetExperienceNeeded(levelComponent.Level + 1);
            progressBar.Value = levelComponent.Experience;
        }
    }
}
