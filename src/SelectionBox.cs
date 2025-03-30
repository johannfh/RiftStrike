using System.Linq;
using Riftstrike.components;
using Riftstrike.src;

namespace Riftstrike
{
    public partial class SelectionBox : Area2D
    {
        private bool IsDragging = false;

        private Panel Panel => GetNode<Panel>("Panel");
        private CollisionShape2D Collider
            => GetNode<CollisionShape2D>("CollisionShape2D");
        private RectangleShape2D CollisionShape
            => Collider.Shape as RectangleShape2D;

        public override void _Ready()
        {
            base._Ready();
            // Timer.Timeout is emitted when the UnitSelectionManager did not
            // handle the left_click (e.g. longer than X milliseconds debounce)
            UnitManager.Instance.DragStart += () => IsDragging = true;
        }

        private Vector2 start = Vector2.Zero;
        private Vector2 end = Vector2.Zero;

        public override void _Process(double delta)
        {
            if (Game.WaveOver || GetTree().Paused)
            {
                IsDragging = false;
                return;
            }

            base._Process(delta);
            var mousePos = GetGlobalMousePosition();
            if (Input.IsActionJustPressed("left_click")) start = mousePos;
            end = mousePos;

            Panel.Visible = IsDragging;
            Collider.Disabled = !IsDragging;

            // Update shapes
            var topLeft = IsDragging ? start.Min(end) : Vector2.Zero;
            var bottomRight = IsDragging ? start.Max(end) : Vector2.Zero;

            SetDimensions(topLeft, bottomRight - topLeft);

            if (Input.IsActionJustReleased("left_click"))
            {
                if (!IsDragging) return;
                GD.Print("Drag end");
                IsDragging = false;
                var append = Input.IsActionPressed("shift");
                var unitsSelected = UnitManager.Instance.unitsSelected;
                if (!append) unitsSelected.Clear();

                var newUnitSelection = GetOverlappingAreas()
                    .OfType<SelectableComponent>()
                    .Select(c => c.unit)
                    .Where(u => !unitsSelected.Contains(u));

                unitsSelected.AddRange(newUnitSelection);
                GD.Print($"Units selected: [{string.Join(", ", unitsSelected.Select(u => u.Name))}]"); ;
            }
        }

        private void SetDimensions(Vector2 position, Vector2 size)
        {
            Panel.Size = size;
            CollisionShape.Size = size;

            Panel.Position = position;
            Collider.Position = position + (size / 2); // because of center alignment
        }
    }
}
