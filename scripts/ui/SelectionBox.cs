using System.IO;
using Godot;
using Godot.Collections;
using Riftstrike.scripts.commands;
using Riftstrike.scripts.components;

namespace Riftstrike.scripts.ui {
    public partial class SelectionBox : Area2D {
        [Signal] public delegate void OnSelectionEventHandler(Array<SelectionComponent> selections);

        private Panel panel;
        private CollisionShape2D collisionShape;

        private bool active;
        public bool Active {
            get => active;
            set {
                // disable/enable panel visibility
                // and collision shape collision detection
                collisionShape.Disabled = !value;
                panel.Visible = value;
                // reset dimensions to zero values when inactive
                if (!value) {
                    SetDimensions(Vector2.Zero, Vector2.Zero);
                }
                // update active state
                active = value;
            }
        }

        public override void _Ready() {
            KeyMap.GetShortcut(CommandType.Move);
            panel = GetNode<Panel>("Panel");
            collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
            Active = false;
        }

        private Vector2 start = Vector2.Zero;
        private Vector2 end = Vector2.Zero;

        public override void _Process(double delta) {
            if (Input.IsActionJustPressed("left_click")) {
                Active = true;
                start = GetGlobalMousePosition();
            }
            if (Active) {
                end = GetGlobalMousePosition();

                // update dimensions to new size
                var topLeft = start.Min(end);
                var bottomRight = start.Max(end);
                SetDimensions(topLeft, bottomRight - topLeft);
            }
            if (Input.IsActionJustReleased("left_click")) {
                var selections = GetOverlappingSelectionComponents();
                EmitSignal(SignalName.OnSelection, selections);
                GD.Print(selections);
                Active = false;
            }
        }

        private Array<SelectionComponent> GetOverlappingSelectionComponents() {
            var overlappingAreas = GetOverlappingAreas();
            var selectionComponents = new Array<SelectionComponent>();

            foreach (var area in overlappingAreas) {
                if (area is SelectionComponent s) {
                    selectionComponents.Add(s);
                }
            }

            return selectionComponents;
        }



        private void SetDimensions(Vector2 position, Vector2 size) {
            panel.Size = size;
            (collisionShape.Shape as RectangleShape2D).Size = size;

            panel.Position = position;
            collisionShape.Position = position + (size / 2);
        }
    }
}
