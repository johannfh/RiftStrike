using Godot;
using Godot.Collections;
using Riftstrike.scripts.commands;
using System.Collections.Generic;

namespace Riftstrike.scripts.components {
    public interface ICommandable {
        public CommandComponent GetCommandComponent();
    }

    [GlobalClass]
    public partial class CommandComponent : Node {
        [Export] public Array<CommandType> supported = new();

        private readonly List<Command> commands = new();

        public List<Command> Commands { get => commands; }

        public void AddCommand(Command command, bool append) {
            if (!append) {
                commands.Clear();
            }
            commands.Add(command);
        }
    }

}