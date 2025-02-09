using Godot;

namespace Riftstrike.scripts.game.ui {
    [GlobalClass]
    public partial class CommandShortcut : Resource {
        [Export] public Shortcut shortcut;
        [Export] public string command;
        [Export] public CommandShortcutType shortcutType;

        public enum CommandShortcutType {
            Notification,
            Targeted,
        }
    }
}
