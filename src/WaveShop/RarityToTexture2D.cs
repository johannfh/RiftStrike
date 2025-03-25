using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    [GlobalClass]
    public partial class RarityToTexture2D : Resource
    {
        [Export]
        public Rarity Rarity;

        [Export]
        public Texture2D Texture;
    }
}
