using Godot;

namespace Riftstrike.src.WaveShop
{
    public partial class WaveShop : Node2D
    {
        [Export]
        private ItemShopUI ItemShopUI;

        [Export]
        private UpgradeShopUI UpgradeShopUI;

        public override void _Ready()
        {
            base._Ready();
            UpgradeShopUI.AllUpgradesPurchased += ToItemShop;
            UpgradeShopUI.GetNextUpgrades();
        }

        private void ToItemShop()
        {
            UpgradeShopUI.Visible = false;
            ItemShopUI.Visible = true;
        }
    }
}
