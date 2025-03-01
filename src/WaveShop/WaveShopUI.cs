using Godot;

namespace Riftstrike.src.WaveShop
{
    public partial class WaveShopUI : Control
    {
        [Export]
        private UpgradeShop UpgradeShop;

        [Export]
        private ItemShop ItemShop;

        public override void _Ready()
        {
            base._Ready();
            CursorSettings.Instance.Cursor = Cursor.Default;
            UpgradeShop.AllUpgradesPurchased += ToItemShop;
        }

        private void ToItemShop()
        {
            UpgradeShop.Visible = false;
            ItemShop.Visible = true;
        }
    }
}
