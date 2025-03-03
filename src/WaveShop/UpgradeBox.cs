using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.upgrades;

namespace Riftstrike.src.WaveShop
{
    public partial class UpgradeBox : Control
    {

        [Export]
        private Array<RarityToTexture2D> RarityTextures = new();

        [Export]
        private TextureRect RarityTextureRect;

        [Export]
        private Button ChooseUpgradeButton;

        [Export]
        private Label UpgradeLabel;

        [Export]
        private AnimationPlayer AnimationPlayer;

        [Signal]
        public delegate void ChooseUpgradeEventHandler(Upgrade upgrade);

#nullable enable

        private Upgrade? upgrade;

        [Export]
        public Upgrade? Upgrade
        {
            get => upgrade;
            set
            {
                upgrade = value;

                Debug.Print($"{nameof(Upgrade)} set to {value?.ResourcePath ?? "UNKNOWN"}");
                AnimationPlayer.Play("RESET");

                // upgrade icon texture
                ChooseUpgradeButton.Icon = value?.Icon;
                ChooseUpgradeButton.Scale = Vector2.One;
                UpgradeLabel.Text = Upgrade?.Label ?? "";

                // upgrade rarity texture
                var rarityTexture = RarityTextures.First(rt => rt.Rarity == value?.Rarity);
                RarityTextureRect.Texture = rarityTexture.Texture;
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ButtonScaleTweenHover(ChooseUpgradeButton, 1.5F, 0.3, 0.1);
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

        public override void _Ready()
        {
            base._Ready();
            ChooseUpgradeButton.Pressed += () =>
            {
                AnimationPlayer.Stop();
                AnimationPlayer.Play("click_ChooseUpgradeButton");
            };

            AnimationPlayer.AnimationFinished += anim =>
            {
                if (anim != "click_ChooseUpgradeButton" || Upgrade == null) return;
                EmitSignal(SignalName.ChooseUpgrade, Upgrade);
            };
        }
    }
}
