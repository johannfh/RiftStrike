using Godot;

namespace Riftstrike
{
    public partial class CursorSettings : Node
    {
        public static CursorSettings Instance { get; private set; }
        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
            cursorDefault = GD.Load<Texture2D>($"{cursorPath}/cursor_default.png");
            cursorPointer = GD.Load<Texture2D>($"{cursorPath}/cursor_pointer.png");
            cursorSelect = GD.Load<Texture2D>($"{cursorPath}/cursor_select.png");
            Cursor = Cursor.Default;
        }
        private static readonly string cursorPath = "res://assets/cursors";
        private Texture2D cursorDefault;
        private Texture2D cursorPointer;
        private Texture2D cursorSelect;
        private Cursor cursor = Cursor.Default;
        public Cursor Cursor
        {
            get => cursor;
            set
            {
                cursor = value;
                Texture2D cursorTexture = cursor switch
                {
                    Cursor.Pointer => cursorPointer,
                    Cursor.Select => cursorSelect,
                    _ => cursorDefault,
                };
                // hotspot is image center
                var cursorHotspot = cursorTexture.GetSize() / 2;
                Input.SetCustomMouseCursor(cursorTexture, hotspot: cursorHotspot);
            }
        }
    }
    public enum Cursor
    {
        Default,
        Pointer,
        Select,
    }
}