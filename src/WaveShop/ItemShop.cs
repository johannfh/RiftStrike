namespace Riftstrike.src.WaveShop
{
    public partial class ItemShop : CenterContainer
    {
        [Export]
        private Button NextWaveButton;

        [Export]
        private AudioStreamPlayer PopSoundAudioStreamPlayer;

        public override void _Ready()
        {
            base._Ready();
            NextWaveButton.Pressed += () =>
            {
                GlobalState.Wave++;

                // move player to scene root to make sounds finish
                PopSoundAudioStreamPlayer.Reparent(GetTree().Root);
                PopSoundAudioStreamPlayer.Finished += PopSoundAudioStreamPlayer.QueueFree;
                PopSoundAudioStreamPlayer.Play();

                GetTree().ChangeSceneToPacked(Game.Scene);
            };
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ButtonScaleTweenHover(NextWaveButton, 1.3F, 0.2, 0.1);
        }

        private void ButtonScaleTweenHover(Button button, float scale, double duration, double revertDuration)
        {
            if (button.ButtonPressed)
            {
                Tween(button, "scale", Vector2.One * ((scale - 1) / 2 + 1), duration);
            }
            else if (button.IsHovered())
            {
                Tween(button, "scale", Vector2.One * scale, duration);
            }
            else
            {
                Tween(button, "scale", Vector2.One, revertDuration);
            }
        }

        private void Tween(GodotObject obj, NodePath property, Variant amount, double duration)
        {
            var tween = CreateTween();
            tween.TweenProperty(obj, property, amount, duration);
        }
    }
}
