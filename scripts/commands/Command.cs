using Godot;

namespace Riftstrike.scripts.commands {
	[GlobalClass]
	public abstract partial class Command : GodotObject { }

    public enum CommandType {
        Stop,
        Move,
        Attack,
    }
}
