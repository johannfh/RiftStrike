using Godot;

namespace Riftstrike.src.WaveShop
{
    public partial class ItemShop : CenterContainer
    {
        [Export]
        private Button NextWaveButton;

        public override void _Ready()
        {
            base._Ready();
            NextWaveButton.Pressed += () =>
            {
                GlobalState.Wave++;
                GetTree().ChangeSceneToPacked(SceneLoader.GameScene);
            };
        }
    }
}
