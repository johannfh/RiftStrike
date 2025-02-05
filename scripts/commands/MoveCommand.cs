using Godot;

namespace Riftstrike.scripts.commands {
    [GlobalClass]
    public partial class MoveCommand : Command {
        public Vector2 target;

        public MoveCommand() {
            target = Vector2.Zero;
        }

        public MoveCommand(Vector2 target) {
            this.target = target;
        }
    }
}
