using Godot;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeBox : Control
    {
        [Export]
        public Upgrade Upgrade;

        [Signal]
        public delegate void ChooseUpgradeEventHandler(Upgrade upgrade);

        public override void _Ready()
        {
            base._Ready();
            GetNode<TextureRect>("%IconTextureRect").Texture = Upgrade.Icon;
            GetNode<Button>("%ChooseUpgradeButton").Pressed += () =>
            {
                EmitSignal(SignalName.ChooseUpgrade, Upgrade);
            };

            ChooseUpgrade += u => GD.Print($"Upgrade {u.ResourcePath} chosen");
        }

        public static UpgradeBox New(Upgrade upgrade)
        {
            var upgradeBoxScene = GD.Load<PackedScene>("res://src/WaveShop/upgrade_box.tscn");

            var upgradeBox = upgradeBoxScene.Instantiate<UpgradeBox>();
            upgradeBox.Upgrade = upgrade;

            return upgradeBox;
        }
    }
}
