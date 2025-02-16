using Godot;
using System;

namespace Riftstrike.components
{
    public partial class LevelDisplay : Control
    {
        [Export]
        private LevelComponent levelComponent;

        [Export]
        private Label label;

        public override void _Process(double delta)
        {
            base._Process(delta);
            label.Text = $"{levelComponent.Level}";
        }
    }
}
