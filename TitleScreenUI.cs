using Godot;

namespace Riftstrike
{
    public partial class TitleScreenUI : Control
    {
        [Export]
        private Button PlayButton;

        public override void _Ready()
        {
            base._Ready();
            // TODO: handle scene changes centralized
            PlayButton.Pressed += () => GetTree().ChangeSceneToFile("res://game.tscn");
        }
    }
}
