using System.Buffers.Binary;
using Godot;

namespace Riftstrike.scripts.ui {
    [GlobalClass]
    public partial class UnitControl : Control {
        private SelectionBox selectionBox;

        [Export] public Camera2D playerCamera;

        public override void _Ready() {
            selectionBox = GetNode<SelectionBox>("SelectionBox");
        }

        public override void _Process(double delta) {
            
        }
    }
}