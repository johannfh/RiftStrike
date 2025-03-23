using Godot;

namespace Riftstrike.Src.Globals.DamageNumbersDisplay
{
    public partial class DamageNumber : Node2D
    {
        [Export]
        private Label DamageNumberLabel;

        [Export]
        private Timer DecayTimer;

        private double damageValue;
        public double DamageValue
        {
            get => damageValue;
            set
            {
                damageValue = value;
                if (IsNodeReady()) UpdateDamageNumberText();
                else updateDamageNumberTextQueued = true;
            }
        }


        private Color fontColor;
        public Color FontColor
        {
            get => fontColor;
            set
            {
                fontColor = value;
                if (IsNodeReady()) UpdateFontColor();
                else updateFontColorQueued = true;
            }
        }

        private int fontSize;
        public int FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                if (IsNodeReady()) UpdateFontSize();
                else updateFontSizeQueued = true;
            }
        }

        private bool updateDamageNumberTextQueued;
        private bool updateFontColorQueued;
        private bool updateFontSizeQueued;

        public override void _Ready()
        {
            base._Ready();
            if (updateDamageNumberTextQueued) UpdateDamageNumberText();
            if (updateFontColorQueued) UpdateFontColor();
            if (updateFontSizeQueued) UpdateFontSize();
            DecayTimer.Timeout += () => QueueFree();
        }

        private void UpdateFontSize()
            => DamageNumberLabel.LabelSettings.FontSize = FontSize;

        private void UpdateFontColor()
            => DamageNumberLabel.LabelSettings.FontColor = FontColor;

        private void UpdateDamageNumberText()
            => DamageNumberLabel.Text = DamageValue.ToString();
    }
}