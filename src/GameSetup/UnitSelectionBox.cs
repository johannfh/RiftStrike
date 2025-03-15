using Godot;

namespace Riftstrike.src.GameSetup
{
    public partial class UnitSelectionBox : AspectRatioContainer
    {
        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    AnimationPlayer.Play($"selected_{value}");
                }
            }
        }

        [Signal]
        public delegate void SelectedEventHandler(bool selected);

        [Export]
        public Texture2D Icon;

        [ExportGroup("Internal")]
        [Export]
        private TextureRect TextureRect;

        [Export]
        private Button SelectButton;

        [Export]
        private AnimationPlayer AnimationPlayer;

        public override void _Ready()
        {
            base._Ready();
            SelectButton.Pressed += () => EmitSignal(SignalName.Selected, !IsSelected);

            TextureRect.Texture = Icon;
            AnimationPlayer.Play("RESET");
        }
    }
}
