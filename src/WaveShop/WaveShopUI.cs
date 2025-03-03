using System.Linq;
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
            UpgradeShop.AllUpgradesPurchased += ToItemShop;

            // directly jump to item shop when there are no remaining levelups
            if (GlobalState.UnitData.Select(ud => ud.RemainingLevelups.Any()).Any()) UpgradeShop.GetNextUpgrades();
            else ToItemShop();
        }

        private void ToItemShop()
        {
            UpgradeShop.Visible = false;
            ItemShop.Visible = true;
        }
    }
}
