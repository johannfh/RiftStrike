using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using Riftstrike.src.units;
using Riftstrike.Src.Globals;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeShop : MarginContainer
    {
        [Export]
        private Array<UpgradeBox> UpgradeBoxes = [];

        [Export]
        private Button RerollUpgradesButton;

        [Export]
        private AnimationPlayer AnimationPlayer;

        [Export]
        private Label RerollCostsLabel;

        [Export]
        private TextureRect CurrentUnitIconTextureRect;

        [Signal]
        public delegate void AllUpgradesPurchasedEventHandler();

        [Export]
        private UnitStatsDisplay UnitStatsDisplay;

#nullable enable

        private ulong NextRerollCosts = 1;

        public override void _Ready()
        {
            base._Ready();

            foreach (var upgradeBox in UpgradeBoxes)
                upgradeBox.ChooseUpgrade += ChooseUpgradeHandler;

            RerollUpgradesButton.MouseEntered += GlobalAudioStreamPlayer.PlayUIElementHoveredSound;

            RerollUpgradesButton.Pressed += () =>
            {
                GlobalAudioStreamPlayer.PlayUIElementPressedSound();
                var insufficientShards = GlobalState.RiftShards < NextRerollCosts;
                var animationPlaying = AnimationPlayer.CurrentAnimation == "click_RerollUpgradesButton";
                var upgradeChosen = UpgradeBoxes.Any(u => u.Chosen);
                var upgradePoolEmpty = !GlobalState.UnitData.Any(u => u.RemainingLevelups.Count != 0);

                if (insufficientShards || animationPlaying || upgradeChosen || upgradePoolEmpty) return;
                GlobalState.RiftShards -= NextRerollCosts;
                NextRerollCosts += 1;

                AnimationPlayer.Stop();
                AnimationPlayer.Play("click_RerollUpgradesButton");
            };

            AnimationPlayer.AnimationFinished += anim =>
            {
                if (anim == "click_RerollUpgradesButton") GetNextUpgrades();
            };
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ButtonScaleTweenHover(RerollUpgradesButton, 1.2F, 0.2, 0.1);
            RerollCostsLabel.Text = $"{NextRerollCosts}";
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
        private UnitData? CurrentUnitData
        {
            get => currentUnitData;
            set
            {
                currentUnitData = value;
                if (value == null)
                {
                    // hide ui elements
                    UnitStatsDisplay.Hide();
                    CurrentUnitIconTextureRect.Hide();
                }
                else
                {
                    // show ui elements
                    UnitStatsDisplay.Show();
                    CurrentUnitIconTextureRect.Show();

                    UnitStatsDisplay.Stats = Stats.From(value.BaseStats, value.Upgrades);
                }
            }
        }

        private bool hadUpgrades = false;

        public void GetNextUpgrades()
        {
            var unitsWithLevelups = GlobalState.UnitData
                .Where(u => u.RemainingLevelups.Count != 0);

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

            CurrentUnitData = unitsWithLevelups.First();
            CurrentUnitIconTextureRect.Texture = CurrentUnitData.Icon;

            GD.Print($"Levelups: {CurrentUnitData.RemainingLevelups.Count}");

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
            if (CurrentUnitData == null || CurrentUnitData.RemainingLevelups.Count == 0) return;
            GD.Print($"Upgrade {upgrade.ResourcePath} chosen");

            // apply upgrade
            CurrentUnitData.Upgrades.Add(upgrade);

            // pop levelup from array
            CurrentUnitData.RemainingLevelups.RemoveAt(0);

            // reset value
            CurrentUnitData = null;

            // get new upgrades
            GetNextUpgrades();
        }
    }
}
