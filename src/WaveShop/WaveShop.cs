using Godot;
using Riftstrike.upgrades;

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
            CursorSettings.Instance.Cursor = Cursor.Default;
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
