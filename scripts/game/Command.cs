#nullable enable
using Godot;

namespace Riftstrike.scripts.game {
    public partial class Command : GodotObject {
        public string Type;
        public string? TargetPosition;
        public string? TargetUnit;

        public Command(string type) {
            Type = type;
        }
    }
}