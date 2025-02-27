using Godot;

namespace Riftstrike.src.WaveShop
{
    public partial class ItemShopUI : Control
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
