using Riftstrike.Src.Globals;

namespace Riftstrike.src.GameSetup
{
    public partial class UnitSelectionBox : AspectRatioContainer
    {
        private const string SCENE_PATH = "res://src/GameSetup/unit_selection_box.tscn";

        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);

        public static UnitSelectionBox New()
            => Scene.Instantiate<UnitSelectionBox>();

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
            SelectButton.MouseEntered += GlobalAudioStreamPlayer.PlayUIElementHoveredSound;

            SelectButton.Pressed += () =>
            {
                GlobalAudioStreamPlayer.PlayUIElementPressedSound();
                EmitSignal(SignalName.Selected, !IsSelected);
            };

            TextureRect.Texture = Icon;
            AnimationPlayer.Play("RESET");
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ButtonScaleTweenHover(SelectButton, 1.3F, 0.3, 0.2);
        }


        private void ButtonScaleTweenHover(Button button, float scale, double duration, double revertDuration)
        {
            if (button.ButtonPressed)
                Tween(button, "scale", Vector2.One * ((scale - 1) / 2 + 1), duration);
            else if (button.IsHovered())
                Tween(button, "scale", Vector2.One * scale, duration);
            else
                Tween(button, "scale", Vector2.One, revertDuration);
        }

        private void Tween(GodotObject obj, NodePath property, Variant amount, double duration)
        {
            var tween = CreateTween();
            tween.TweenProperty(obj, property, amount, duration);
        }
    }
}
