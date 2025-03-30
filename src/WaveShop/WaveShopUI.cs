using System.Linq;

namespace Riftstrike.src.WaveShop
{
    public partial class WaveShopUI : Control
    {
        [Export]
        private UpgradeShop UpgradeShop;

        [Export]
        private ItemShop ItemShop;

        [Signal]
        public delegate void NextWaveEventHandler();

        public void Load()
        {
            UpgradeShop.NextRerollCosts = (ulong)Mathf.Max(UpgradeShop.NextRerollCosts * 0.8, 1);


            // directly jump to item shop when there are no remaining levelups
            if (GlobalState.UnitData.Select(ud => ud.RemainingLevelups.Count != 0).Any())
            {
                UpgradeShop.Visible = true;
                ItemShop.Visible = false;
                UpgradeShop.GetNextUpgrades();
            }
            else
            {
                Debug.Print("No levelups this round!");
                ToItemShop();
            }
        }

        public override void _Ready()
        {
            base._Ready();
            UpgradeShop.AllUpgradesPurchased += ToItemShop;
            ItemShop.NextWave += () => EmitSignal(SignalName.NextWave);
        }

        private void ToItemShop()
        {
            UpgradeShop.Visible = false;
            ItemShop.Visible = true;
        }
    }
}
