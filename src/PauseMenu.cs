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
        }
    }
}
