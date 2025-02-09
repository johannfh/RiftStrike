using System;
using Godot;

namespace Riftstrike.scripts.game {
	public partial class Camera : Camera2D {
		[ExportGroup("Move Parameters")]
		[Export(PropertyHint.Range, "0.1,1,0.05")] public float MoveSensitivity;
		[Export(PropertyHint.Range, "1,30,1")] public int MovePixelThresold;

		[ExportGroup("Zoom Parameters")]
		[Export(PropertyHint.Range, "0.05,0.5,0.05")] public float ZoomSensitivity;
		[Export(PropertyHint.Range, "0.1,1,0.1")] public float ZoomMinimum;
		[Export(PropertyHint.Range, "1,3,0.1")] public float ZoomMaximum;

		public override void _Process(double delta) {
			base._Process(delta);
			HandleZoom();
			HandleMove();
		}

		private void HandleMove() {
			var mousePos = GetGlobalMousePosition();
			var screenRect = GetViewportRect();
			// GD.Print(screenRect.Size);
		}

		private void HandleZoom() {
			var directionIn = Input.IsActionJustReleased("zoom_in") ? 1 : 0;
			var directionOut = Input.IsActionJustReleased("zoom_out") ? -1 : 0;
			var scrollDirection = directionIn + directionOut;
			var zoomVelocity = scrollDirection * ZoomSensitivity;
			var newZoom = Zoom + new Vector2(zoomVelocity, zoomVelocity);
			Zoom = newZoom.Clamp(
				new Vector2(ZoomMinimum, ZoomMinimum),
				new Vector2(ZoomMaximum, ZoomMaximum)
			);
			if (Input.IsActionJustReleased("zoom_in") || Input.IsActionJustReleased("zoom_out")) {
				GD.Print($"ZOOM to {Zoom}");
			}
		}
	}
}
