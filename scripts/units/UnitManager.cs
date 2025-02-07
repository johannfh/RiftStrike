using Godot;
using Godot.Collections;

namespace Riftstrike.scripts.units {
    [GlobalClass]
    public partial class UnitManager : Node
    {
        public Array<Unit> Units {
            get {
                var children = GetChildren();
                var units = new Array<Unit>();

                foreach (var child in children) {
                    if (child is Unit unit) {
                        units.Add(unit);
                    }
                }
                
                return units;
            }
        }

        public void AddUnit(Unit unit) {
            AddChild(unit);
        }
    }
}
