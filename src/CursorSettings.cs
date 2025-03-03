using Godot;

namespace Riftstrike.src
{
    [GlobalClass]
    public partial class CursorSettings : GodotObject
    {
        private const string CURSOR_PATH = "res://src/assets/cursors";

        private static Texture2D CursorDefault
            => GD.Load<Texture2D>($"{CURSOR_PATH}/cursor_default.png");

        private static Texture2D CursorPointer
            => GD.Load<Texture2D>($"{CURSOR_PATH}/cursor_pointer.png");

        private static Texture2D CursorSelect
            => GD.Load<Texture2D>($"{CURSOR_PATH}/cursor_select.png");

        public static void LoadCursors()
        {
            Input.SetCustomMouseCursor(
                CursorDefault,
                Input.CursorShape.Arrow,
                CursorDefault.GetSize() / 2
            );

            Input.SetCustomMouseCursor(
                CursorPointer,
                Input.CursorShape.PointingHand,
                CursorPointer.GetSize() / 2
            );

            Input.SetCustomMouseCursor(
                CursorSelect,
                Input.CursorShape.Cross,
                CursorSelect.GetSize() / 2
            );
        }
    }
}