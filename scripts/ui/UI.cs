using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.scripts.components;
using Riftstrike.scripts.units;

namespace Riftstrike.scripts.ui {
	[GlobalClass]
	public partial class UI : Control {
		private SelectionBox selectionBox;
		private readonly List<Unit> selectedUnits = new();

		[Export] private UnitManager unitManager;

		[ExportGroup("Camera Settings")]
		[Export] private Camera2D camera;
		[Export] private float cameraZoomSensitivity;
		[Export] private float cameraZoomMinimum;
		[Export] private float cameraZoomMaximum;

		public override void _Ready() {
			selectionBox = GetNode<SelectionBox>("SelectionBox");
			selectionBox.OnSelection += OnSelection;
		}

        public override void _Process(double delta) {
            var directionIn = Input.IsActionJustReleased("zoom_in") ? 1 : 0;
			var directionOut = Input.IsActionJustReleased("zoom_out") ? -1 : 0;
			var scrollDirection = directionIn + directionOut;
			var scrollVelocity = scrollDirection * cameraZoomSensitivity;
			camera.Zoom = (camera.Zoom + new Vector2(scrollVelocity, scrollVelocity)).Clamp(
				new Vector2(cameraZoomMinimum, cameraZoomMinimum),
				new Vector2(cameraZoomMaximum, cameraZoomMaximum)
			);
        }

        private void OnSelection(Array<SelectionComponent> selections, bool append) {
			GD.Print($"Units: []");

			if (!append) {
				var newSelectedUnits = selections.Select(s => s.unit);
				// scan for units to deselect
				var unitsToDeselect = selectedUnits.FindAll(unit => !newSelectedUnits.Contains(unit));

				// deselect units
				selectedUnits.RemoveAll(unit => unitsToDeselect.Contains(unit));
				unitsToDeselect.ForEach(unit => {
					(unit as ISelectable).GetSelectionComponent().Selected = false;
					GD.Print($"Deselecting unit {unit.Name}");
				});
			}

			foreach (var selection in selections) {
				var unit = selection.unit;
				if (!selectedUnits.Contains(unit)) {
					selection.Selected = true;
					selectedUnits.Add(unit);
					GD.Print($"Selecting unit {unit.Name}");
				}
			}

			GD.Print($"Units selected ({selectedUnits.Count}): [{string.Join(", ", selectedUnits.Select(u => u.Name))}]");
		}
	}
}
