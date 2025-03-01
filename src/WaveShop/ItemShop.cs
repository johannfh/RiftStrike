using Godot;

namespace Riftstrike.src.WaveShop
{
    public partial class ItemShop : CenterContainer
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
