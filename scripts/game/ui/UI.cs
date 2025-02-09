using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.scripts.game.components;
using Riftstrike.scripts.game.units;

namespace Riftstrike.scripts.game.ui {
	public partial class UI : Control {
		private SelectionBox selectionBox;
		private readonly List<Unit> selectedUnits = new();


		public override void _Ready() {
			selectionBox = GetNode<SelectionBox>("SelectionBox");
			selectionBox.OnSelection += OnSelection;
		}

		private void OnSelection(Array<SelectionComponent> selections, bool append) {
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
