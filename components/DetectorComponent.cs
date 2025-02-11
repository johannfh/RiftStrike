using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Riftstrike.components {
    public partial class DetectorComponent : Area2D {
        public IEnumerable<DetectableComponent> Detect()
            => GetOverlappingAreas()
                .OfType<DetectableComponent>();
    }
}
