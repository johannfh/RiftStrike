using Godot;
using Godot.Collections;

namespace Riftstrike.scripts.game.components {
    public interface ICommandable {
        CommandComponent GetCommandComponent();
    }

    [GlobalClass]
    public partial class CommandComponent : Node {
        [Export] public Array<string> supportedCommandTypes = new();

        [Signal] public delegate void OnCommandEventHandler(Command command, bool append);
        
        public void AddCommand(Command command, bool append) {
            EmitSignal(SignalName.OnCommand, command, append);
        }
    }
}