using Godot;

namespace Riftstrike.src
{
    public partial class WaveShopUI : Control
    {
        public override void _Ready()
        {
            base._Ready();
            GetNode<Button>("%NextWaveButton").Pressed += () =>
            {
                GlobalState.Wave++;
                GetTree().ChangeSceneToPacked(SceneLoader.GameScene);
            };
        }
    }
}
