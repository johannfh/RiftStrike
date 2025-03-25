using Riftstrike.src;
using Riftstrike.src.GameSetup;

namespace Riftstrike
{
    public partial class TitleScreenUI : Control
    {
        private const string SCENE_PATH = "res://src/title_screen_ui.tscn";

        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);

        [Export]
        private Button PlayButton;

        public override void _Ready()
        {
            base._Ready();
            CursorSettings.LoadCursors();

            PlayButton.Pressed += () =>
            {
                GetTree().ChangeSceneToPacked(GameSetup.Scene);
            };
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ButtonScaleTweenHover(PlayButton, 1.3F, 0.2, 0.1);
        }

        private void ButtonScaleTweenHover(Button button, float scale, double duration)
            => ButtonScaleTweenHover(button, scale, duration, duration);

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
    }
}
