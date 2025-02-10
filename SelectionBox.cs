using System;
using Godot;

namespace Riftstrike {
    public partial class SelectionBox : Area2D {
        private bool Active = false;

        private Panel Panel => GetNode<Panel>("Panel");
        private CollisionShape2D Collider
            => GetNode<CollisionShape2D>("CollisionShape2D");
        private RectangleShape2D CollisionShape
            => Collider.Shape as RectangleShape2D;

        public override void _Ready() {
            base._Ready();
            // Timer.Timeout is emitted when the UnitSelectionManager did not
            // handle the left_click (e.g. longer than X milliseconds debounce)
            UnitSelectionManager.Instance.SelectUnitTimer.Timeout += () => Active = true;
        }

        private Vector2 start = Vector2.Zero;
        private Vector2 end = Vector2.Zero;

        public override void _Process(double delta) {
            base._Process(delta);
            var mousePos = GetGlobalMousePosition();
            if (Input.IsActionJustPressed("left_click")) start = mousePos;
            end = mousePos;

            // TODO: HITBOX AND VISUAL UPDATES
            Panel.Visible = Active;
            Collider.Disabled = !Active;

            // Update shapes
            var topLeft = start.Min(end);
            var bottomRight = start.Max(end);

            if (Active) {
                SetDimensions(topLeft, bottomRight - topLeft);
            }

            if (Input.IsActionJustReleased("left_click")) {
                Active = false;
                var append = Input.IsKeyPressed(Key.Shift);

                // TODO: SELECTION
                
            }
        }

        private void SetDimensions(Vector2 position, Vector2 size) {
            Panel.Size = size;
            CollisionShape.Size = size;

            Panel.Position = position;
            Collider.Position = position + (size / 2); // because of center alignment
        }
    }
}
