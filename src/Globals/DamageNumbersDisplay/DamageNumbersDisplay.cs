using Riftstrike.src;
using System;

namespace Riftstrike.Src.Globals.DamageNumbersDisplay
{
    public partial class DamageNumbersDisplay : Node
    {
        private static DamageNumbersDisplay Instance;

        [Export]
        private Gradient FontDamageGradient;

        [Export]
        private PackedScene DamageNumberScene;

        [Export]
        private int LowestFontSize;

        [Export]
        private int HighestFontSize;

        public override void _Ready()
        {
            base._Ready();
            if (Instance != this && Instance != null)
            {
                QueueFree();
                return;
            }
            Instance = this;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            if (Instance == this) Instance = null;
        }

        public static void DisplayNumber(double damage, Vector2 position)
        {
            var damageNumber = Instance.DamageNumberScene.Instantiate<DamageNumber>();
            GlobalState.HighestDamage = Math.Max(damage, GlobalState.HighestDamage);

            var delta = damage / GlobalState.HighestDamage;

            var color = Instance.FontDamageGradient.Sample((float)delta);
            var fontSize = MathUtils.Lerp(Instance.LowestFontSize, Instance.HighestFontSize, delta);

            damageNumber.GlobalPosition = position;
            damageNumber.DamageValue = damage;
            damageNumber.FontColor = color;
            damageNumber.FontSize = fontSize;

            Instance.AddChild(damageNumber);
        }
    }
}
