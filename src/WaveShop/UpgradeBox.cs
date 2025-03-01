using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeBox : Control
    {
        private Upgrade upgrade;

        [Export]
        private Array<RarityToTexture2D> RarityTextures = new();

        [Signal]
        public delegate void ChooseUpgradeEventHandler(Upgrade upgrade);

        [Export]
        public Upgrade Upgrade
        {
            get => upgrade;
            set
            {
                upgrade = value;

                // upgrade icon texture
                GetNode<TextureRect>("%IconTextureRect").Texture = Upgrade.Icon;

                // upgrade rarity texture
                var rarityTexture = RarityTextures.First(rt => rt.Rarity == Upgrade.Rarity);
                GetNode<TextureRect>("%RarityTextureRect").Texture = rarityTexture.Texture;
            }
        }

        public override void _Ready()
        {
            base._Ready();
            GetNode<Button>("%ChooseUpgradeButton").Pressed += () =>
            {
                EmitSignal(SignalName.ChooseUpgrade, Upgrade);
            };
        }
    }
}
