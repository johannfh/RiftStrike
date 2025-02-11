using Godot;
using System;

namespace Riftstrike.components {
    public partial class DetectableComponent : Area2D {
        [Export] public Node Parent { get; private set; }
    }
}
