using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.src.units;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeShop : MarginContainer
    {
        const int UPGRADE_COUNT = 4;

        [Signal]
        public delegate void AllUpgradesPurchasedEventHandler();

        public override void _Ready()
        {
            base._Ready();
            GetNode<Button>("%RerollUpgradesButton").Pressed += RerollUpgrades;
        }

        private void RerollUpgrades()
        {
            GetNextUpgrades();
        }

        public void GetNextUpgrades()
        {
            var upgradesContainer = GetNode<HBoxContainer>("%UpgradesContainer");

            // remove old upgrades
            upgradesContainer.GetChildren()
                .ForEach(node => node.QueueFree());

            var unitsWithLevelups = GlobalState.UnitData
                .Where(u => u.RemainingLevelups.Any());

            GD.Print($"Units with levelups: {unitsWithLevelups.Count()}");

            if (!unitsWithLevelups.Any())
            {
                GD.Print("No more units with levelups!");
                EmitSignal(SignalName.AllUpgradesPurchased);
                return;
            }

            var unitData = unitsWithLevelups.First();

            GD.Print($"Levelups: {unitData.RemainingLevelups.Count}");

            // generate upgrades for levelup
            var upgrades = new List<Upgrade>();
            for (int i = 0; i < UPGRADE_COUNT; i++)
            {
                var randomUpgrade = UpgradeFactory.RandomLevelupUpgrade();
                Debug.Assert(randomUpgrade != null, $"{nameof(randomUpgrade)} is null");
                GD.Print($"Got upgrade: {randomUpgrade.ResourcePath}");
                upgrades.Add(randomUpgrade);
            }

            // render new upgrades
            foreach (var upgrade in upgrades)
            {
                var upgradeBox = UpgradeBox.New(upgrade);

                upgradeBox.ChooseUpgrade += upgrade =>
                {
                    // apply upgrade
                    unitData.Upgrades.Add(upgrade);

                    // pop levelup from array
                    unitData.RemainingLevelups.RemoveAt(0);

                    // get new upgrades
                    GetNextUpgrades();
                };

                // render upgrade option
                upgradesContainer.AddChild(upgradeBox);
            }
        }
    }
}
