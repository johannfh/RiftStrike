using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeBox : Control
    {
        [Export]
        public Upgrade Upgrade;

        [Export]
        private Array<RarityToTexture2D> RarityTextures = new();

        [Signal]
        public delegate void ChooseUpgradeEventHandler(Upgrade upgrade);

#nullable enable

        public override void _Ready()
        {
            base._Ready();
            GetNode<TextureRect>("%IconTextureRect").Texture = Upgrade.Icon;

            var rarityTexture = RarityTextures.First(rt => rt.Rarity == Upgrade.Rarity);
            GetNode<TextureRect>("%RarityTextureRect").Texture = rarityTexture.Texture;

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
