using Godot;

namespace Riftstrike
{
    public partial class Camera : Camera2D
    {
        [Export] private float movePixelThreshold = 80;
        [Export] private float moveSpeed = 300;

        [ExportCategory("Camera Zoom")]
        [Export(PropertyHint.Range, "0.1,1,0.05,or_greater")] private double zoomMinimum = 0.1;
        [Export(PropertyHint.Range, "1,3,0.05,or_greater")] private double zoomMaximum = 2;
        [Export(PropertyHint.Range, "0.05,0.5,0.05")] private double zoomSensitivity = 0.05;

        private static bool IsDragging = false;

        public override void _Process(double delta)
        {
            HandleDragging();
            // only react to corner movement when
            // not dragging the camera with the mouse
            if (!IsDragging) UpdatePosition(delta);
            HandleZooming();
        }


        private void UpdatePosition(double delta)
        {
            if (!GetWindow().HasFocus()) return;
            var screenRect = GetViewportRect();
            var mousePosV = GetViewportTransform() * GetGlobalMousePosition();
            const float leftOvershoot = 1;
            const float rightOvershoot = 1;
            const float upOvershoot = 1;
            const float downOvershoot = 1;

            var leftDir = (mousePosV.X > -leftOvershoot && mousePosV.X < movePixelThreshold) ||
                Input.IsActionPressed("camera_left") ? -1 : 0;
            var rightDir = (mousePosV.X > screenRect.Size.X - movePixelThreshold && mousePosV.X < screenRect.Size.X + rightOvershoot) ||
                Input.IsActionPressed("camera_right") ? 1 : 0;
            var horizontalDir = leftDir + rightDir;

            var upDir = (mousePosV.Y > -upOvershoot && mousePosV.Y < movePixelThreshold) ||
                Input.IsActionPressed("camera_up") ? -1 : 0;
            var downDir = (mousePosV.Y > screenRect.Size.Y - movePixelThreshold && mousePosV.Y < screenRect.Size.Y + downOvershoot) ||
                Input.IsActionPressed("camera_down") ? 1 : 0;
            var verticalDir = upDir + downDir;

            var direction = new Vector2(horizontalDir, verticalDir);

            GlobalPosition += direction * moveSpeed * (float)delta;
        }

        private Vector2 dragLastPos = Vector2.Zero;

        private void HandleDragging()
        {
            var mousePos = GetGlobalMousePosition();
            if (Input.IsActionJustPressed("drag_camera"))
            {
                IsDragging = true;
                dragLastPos = mousePos;
                GD.Print($"Started dragging at {dragLastPos}");
            }
            if (Input.IsActionJustReleased("drag_camera"))
            {
                IsDragging = false;
                GD.Print($"Stopped dragging at {mousePos}");
            }
            if (IsDragging)
            {
                var dragDiff = mousePos - dragLastPos;
                GlobalPosition -= dragDiff;
                dragLastPos = mousePos - dragDiff;
            }
        }

        private void HandleZooming()
        {
            var directionIn = Input.IsActionJustReleased("zoom_in") ? 1 : 0;
            var directionOut = Input.IsActionJustReleased("zoom_out") ? -1 : 0;
            var zoomDirection = directionIn + directionOut;
            var zoomVelocity = (float)(zoomDirection * zoomSensitivity);
            Zoom = (Zoom + Vector2Extensions.FromValue(zoomVelocity))
                .Clamp((float)zoomMinimum, (float)zoomMaximum);
        }
    }
}
