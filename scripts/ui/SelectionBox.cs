using Godot;
using Riftstrike.scripts.commands;

namespace Riftstrike.scripts.ui {
    public partial class SelectionBox : Node2D {
        public override void _Ready() {
            KeyMap.GetShortcut(CommandType.Move);
        }
    }
}
