using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeBox : Control
    {

        [Export]
        private Array<RarityToTexture2D> RarityTextures = new();

        [Export]
        private TextureRect IconTextureRect;

        [Export]
        private TextureRect RarityTextureRect;

        [Signal]
        public delegate void ChooseUpgradeEventHandler(Upgrade upgrade);

#nullable enable

        private Upgrade? upgrade;

        [Export]
        public Upgrade? Upgrade
        {
            get => upgrade;
            set
            {
                upgrade = value;

                Debug.Print($"{nameof(Upgrade)} set to {value?.ResourcePath ?? "UNKNOWN"}!");

                // upgrade icon texture
                IconTextureRect.Texture = value?.Icon;

                // upgrade rarity texture
                var rarityTexture = RarityTextures.First(rt => rt.Rarity == value?.Rarity);
                RarityTextureRect.Texture = rarityTexture.Texture;
            }
        }

        public override void _Ready()
        {
            base._Ready();
            GetNode<Button>("%ChooseUpgradeButton").Pressed += () =>
            {
                if (Upgrade == null) return;
                EmitSignal(SignalName.ChooseUpgrade, Upgrade);
            };
        }
    }
}
