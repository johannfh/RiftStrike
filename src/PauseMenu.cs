namespace Riftstrike.src
{
    public partial class PauseMenu : Control
    {
        [Signal]
        public delegate void PausedChangedEventHandler(bool paused);

        [Export]
        private Button ContinueButton;

        [Export]
        private Button MainMenuButton;

        private bool paused;
        public bool Paused
        {
            get => paused;
            set
            {
                if (paused == value) return;
                paused = value;
                EmitSignal(SignalName.PausedChanged, paused);
            }
        }

        public void TogglePaused()
            => Paused = !Paused;

        public override void _Ready()
        {
            base._Ready();

            MainMenuButton.Pressed += () =>
            {
                GetTree().Paused = false;
                GetTree().ChangeSceneToPacked(TitleScreenUI.Scene);
            };

            ContinueButton.Pressed += ()
                => Paused = false;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (Input.IsActionJustPressed("pause_game"))
                TogglePaused();
            ButtonScaleTweenHover(ContinueButton, 1.3F, 0.3, 0.2);
            ButtonScaleTweenHover(MainMenuButton, 1.3F, 0.3, 0.2);
        }

        private void ButtonScaleTweenHover(Button button, float scale, double duration, double revertDuration)
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
