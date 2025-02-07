using Godot;
using Riftstrike.scripts.units;

namespace Riftstrike.scripts.components {
    public interface ISelectable {
        public SelectionComponent GetSelectionComponent();
    }

    [GlobalClass]
    public partial class SelectionComponent : Area2D {
        [Signal] public delegate void SelectionChangedEventHandler(bool selected);

        [Export] public Unit unit;

        private bool selected = false;

        public bool Selected {
            get => selected;
            set {
                if (selected != value) {
                    selected = value;
                    EmitSignal(SignalName.SelectionChanged, selected);
                }
            }
        }
    }
}