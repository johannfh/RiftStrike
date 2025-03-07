using Godot;
using System;

namespace Riftstrike.src
{
    public partial class GameUI : Control
    {
        [Export]
        private Timer WaveEndTimer;

        public override void _Process(double delta)
        {
            base._Process(delta);
            var secondsLeft = (int)Math.Round(WaveEndTimer.TimeLeft);
            GetNode<Label>("%WaveDurationLabel").Text = $"{secondsLeft}";
        }
    }
}
