using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.components;

namespace Riftstrike {
	public partial class UnitSelectionManager : Node2D {
		public static UnitSelectionManager Instance { get; set; }

		public override void _Ready() {
			if (Instance != null && Instance != this) {
				QueueFree();
			} else {
				Instance = this;
			}
		}

		public override void _Process(double delta) {
			base._Process(delta);
			if (Input.IsActionJustPressed("left_click")) {
				var mousePos = GetGlobalMousePosition();

				var space_state = GetWorld2D().DirectSpaceState;
				var query = new PhysicsPointQueryParameters2D {
					Position = mousePos,
					CollisionMask = (uint)CollisionLayer.Selection,
					CollideWithAreas = true,
					CollideWithBodies = false,
				};
				var result = space_state.IntersectPoint(query);

				var clickedUnits = result.Select(v => v["collider"].As<ClickableComponent>().unit);

				if (!Input.IsActionPressed("shift")) unitsSelected.Clear();

				unitsSelected.AddRange(clickedUnits);
				GD.Print($"Units selected: [{string.Join(", ", unitsSelected.Select(u => u.Name))}]");
			}

			if (Input.IsActionJustPressed("right_click")) {
				var mousePos = GetGlobalMousePosition();
				unitsSelected.OfType<IWalk>()
					.ForEach(walkable => walkable.WalkTo(mousePos));
			}


		}

		public readonly Array<Unit> units = new();
		public readonly Array<Unit> unitsSelected = new();

	}
}
