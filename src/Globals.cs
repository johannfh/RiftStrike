using Godot;
using Godot.Collections;

namespace Riftstrike.src
{
    public partial class Globals : Node
    {
        private static Globals Instance;

        [Export]
        private Array<PackedScene> loadedScenes = new();

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
            foreach (var scene in loadedScenes)
            {
                var node = scene.Instantiate();
                AddChild(node);
                GD.Print($"Loaded {nameof(node)} {node.Name}");
            }
        }
    }
}