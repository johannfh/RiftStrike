using System.Collections.Generic;
using System.IO;
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

        [Export]
        private Button RerollUpgradesButton;

        [Export]
        private AnimationPlayer AnimationPlayer;

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

            RerollUpgradesButton.Pressed += () => GetNextUpgrades();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ButtonScaleTweenHover(RerollUpgradesButton, 1.2F, 0.2, 0.1);
        }

        private void ButtonScaleTweenHover(Button button, float scale, double duration, double revertDuration)
        {
            if (button.ButtonPressed)
            {
                Tween(button, "scale", Vector2.One * ((scale - 1) / 2 + 1), duration);
            }
            else if (button.IsHovered())
            {
                Tween(button, "scale", Vector2.One * scale, duration);
            }
            else
            {
                Tween(button, "scale", Vector2.One, revertDuration);
            }
        }

        private void Tween(GodotObject obj, NodePath property, Variant amount, double duration)
        {
            var tween = CreateTween();
            tween.TweenProperty(obj, property, amount, duration);
        }


        private UnitData? currentUnitData;

        private bool hadUpgrades = false;

        public void GetNextUpgrades()
        {
            var unitsWithLevelups = GlobalState.UnitData
                .Where(u => u.RemainingLevelups.Any());

            GD.Print($"Units with levelups: {unitsWithLevelups.Count()}");

            if (!unitsWithLevelups.Any())
            {
                GD.Print("No more units with levelups!");

                if (hadUpgrades)
                {
                    // wait for animation to finish before completing scene stage
                    AnimationPlayer.Stop();
                    AnimationPlayer.AnimationFinished += (_) => EmitSignal(SignalName.AllUpgradesPurchased);
                    AnimationPlayer.Play("hide_UpgradeBoxes");
                }
                else
                {
                    EmitSignal(SignalName.AllUpgradesPurchased);
                }

                return;
            }

            hadUpgrades = true;

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

            AnimationPlayer.Play("show_UpgradeBoxes");
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
