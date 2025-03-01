using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.src.units;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeShop : MarginContainer
    {
        [Export]
        private Array<UpgradeBox> UpgradeBoxes = new();

        [Signal]
        public delegate void AllUpgradesPurchasedEventHandler();

#nullable enable

        public override void _Ready()
        {
            base._Ready();
            foreach (var upgradeBox in UpgradeBoxes)
            {
                upgradeBox.ChooseUpgrade += ChooseUpgradeHandler;
            }

            GetNode<Button>("%RerollUpgradesButton").Pressed += () => GetNextUpgrades();
            GetNextUpgrades();
        }

        private UnitData? currentUnitData;

        public void GetNextUpgrades()
        {
            var unitsWithLevelups = GlobalState.UnitData
                .Where(u => u.RemainingLevelups.Any());

            GD.Print($"Units with levelups: {unitsWithLevelups.Count()}");

            if (!unitsWithLevelups.Any())
            {
                GD.Print("No more units with levelups!");
                EmitSignal(SignalName.AllUpgradesPurchased);
                return;
            }

            currentUnitData = unitsWithLevelups.First();

            GD.Print($"Levelups: {currentUnitData.RemainingLevelups.Count}");

            // generate upgrades for levelup
            var upgrades = new List<Upgrade>();
            for (int i = 0; i < UpgradeBoxes.Count; i++)
            {
                Upgrade randomUpgrade;
                while (true)
                {
                    randomUpgrade = UpgradeFactory.RandomLevelupUpgrade();
                    // ensure unique upgrades
                    if (upgrades.Contains(randomUpgrade)) continue;
                    break;
                }
                GD.Print($"Got upgrade: {randomUpgrade.ResourcePath}");
                upgrades.Add(randomUpgrade);
            }

            Debug.Assert(
                upgrades.Count == UpgradeBoxes.Count,
                $"Not enough {nameof(upgrades)} for {nameof(UpgradeBoxes)}"
            );

            // set new upgrades
            for (int i = 0; i < UpgradeBoxes.Count; i++)
            {
                var upgradeBox = UpgradeBoxes.ElementAt(i);
                upgradeBox.Upgrade = upgrades.ElementAt(i);
            }
        }

        private void ChooseUpgradeHandler(Upgrade upgrade)
        {
            if (currentUnitData == null || !currentUnitData.RemainingLevelups.Any()) return;
            GD.Print($"Upgrade {upgrade.ResourcePath} chosen");

            // apply upgrade
            currentUnitData.Upgrades.Add(upgrade);

            // pop levelup from array
            currentUnitData.RemainingLevelups.RemoveAt(0);

            // reset value
            currentUnitData = null;

            // get new upgrades
            GetNextUpgrades();
        }
    }
}
