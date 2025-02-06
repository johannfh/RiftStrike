using Godot;

namespace Riftstrike.scripts.ui {
    [GlobalClass]
    public partial class CommandShortcut : Resource {
        [Export] public Shortcut shortcut;
        [Export] public CommandShortcutType type;
        [Export] public Texture2D commandIcon;
        [Export] public Texture2D shortcutIcon;
    }

    public enum CommandShortcutType {
        Notification,
        Interactive,
    }
}