using Godot;
using Godot.Collections;
using Riftstrike.scripts.commands;
using Riftstrike.scripts.components;

namespace Riftstrike.scripts.ui {
    public partial class SelectionBox : Area2D {
        private Panel panel;
        private CollisionShape2D collisionShape;

        public override void _Ready() {
            KeyMap.GetShortcut(CommandType.Move);
            panel = GetNode<Panel>("Panel");
            collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        }

        public Array<SelectionComponent> GetOverlappingSelectionComponents() {
            var overlappingAreas = GetOverlappingAreas();
            var selectionComponents = new Array<SelectionComponent>();

            foreach (var area in overlappingAreas) {
                if (area is SelectionComponent s) {
                    selectionComponents.Add(s);
                }
            }

            return selectionComponents;
        }

        public void SetState(Vector2 position, Vector2 size) {
            panel.Size = size;
            (collisionShape.Shape as RectangleShape2D).Size = size;

            panel.Position = position;
            collisionShape.Position = position + (size / 2);
        }
    }
}
