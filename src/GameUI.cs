using System;
using Riftstrike.Src.Globals;

namespace Riftstrike.src
{
    public partial class GameUI : Control
    {
        [Export]
        private Timer WaveEndTimer;

        [Export]
        private TextureButton PauseButton;

        [Signal]
        public delegate void PausedEventHandler();

        public override void _Ready()
        {
            base._Ready();

            PauseButton.MouseEntered += GlobalAudioStreamPlayer.PlayUIElementHoveredSound;

            PauseButton.Pressed += () =>
            {
                GlobalAudioStreamPlayer.PlayUIElementPressedSound();
                EmitSignal(SignalName.Paused);
            };
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            var secondsLeft = (int)Math.Ceiling(WaveEndTimer.TimeLeft);
            GetNode<Label>("%WaveDurationLabel").Text = $"{secondsLeft}";
            ButtonScaleTweenHover(PauseButton, 1.3F, 0.3, 0.2);
        }

        private void ButtonScaleTweenHover(TextureButton button, float scale, double duration, double revertDuration)
        {
            if (button.ButtonPressed)
                Tween(button, "scale", Vector2.One * ((scale - 1) / 2 + 1), duration);
            else if (button.IsHovered())
                Tween(button, "scale", Vector2.One * scale, duration);
            else
                Tween(button, "scale", Vector2.One, revertDuration);
        }

        private void Tween(GodotObject obj, NodePath property, Variant amount, double duration)
        {
            var tween = CreateTween();
            tween.TweenProperty(obj, property, amount, duration);
        }
    }
}
