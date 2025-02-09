using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.scripts.game.units;

namespace Riftstrike.scripts.game {
    [GlobalClass]
    public partial class UnitManager : Node {
        public Array<Unit> Units
            => new(GetChildren().OfType<Unit>());

        public void AddUnit(Unit unit) {
            GD.Print($"Registering unit {unit.Name}");
            AddChild(unit);
        }
    }
}