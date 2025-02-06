using Godot;

namespace Riftstrike.scripts.commands {
	[GlobalClass]
	public abstract partial class Command : GodotObject {
        public abstract CommandType GetCommandType();
    }

    public enum CommandType {
        Stop,
        Move,
        Attack,
    }
}
