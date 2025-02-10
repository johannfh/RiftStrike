using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.components;

namespace Riftstrike {
	public partial class UnitSelectionManager : Node2D {
		public static UnitSelectionManager Instance { get; set; }

		// The Timer instance specifies the max duration for a mouse down
		// to count as a click. Longer actions will count as things like Dragging.
		// TODO: Expose a signal that is connected to SelectUnitTimer.Timeout
		// to hide the internals (private Timer; [Signal] public delegate).
		private static readonly double SelectUnitsWaitTime = 0.15;
		public Timer SelectUnitTimer;

		public override void _Ready() {
			base._Ready();
			if (Instance != null && Instance != this) {
				QueueFree();
			} else {
				Instance = this;
			}
            SelectUnitTimer = new Timer {
                WaitTime = SelectUnitsWaitTime,
				OneShot = true,
            };
            AddChild(SelectUnitTimer);
		}

		public override void _Process(double delta) {
			base._Process(delta);
			if (Input.IsActionJustPressed("left_click")) {
				SelectUnitTimer.Start();
			}
			if (Input.IsActionJustReleased("left_click")) {
				if (!SelectUnitTimer.IsStopped()) {
					SelectUnitTimer.Stop();
					HandleSelectUnit();
				}
			}

			if (Input.IsActionJustPressed("right_click")) {
				var mousePos = GetGlobalMousePosition();
				unitsSelected.OfType<IWalk>()
					.ForEach(walkable => walkable.WalkTo(mousePos));
			}
		}

		public readonly Array<Unit> units = new();
		public readonly Array<Unit> unitsSelected = new();

		public void HandleSelectUnit() {
			var mousePos = GetGlobalMousePosition();

			var space_state = GetWorld2D().DirectSpaceState;
			var query = new PhysicsPointQueryParameters2D {
				Position = mousePos,
				CollisionMask = (uint)CollisionLayer.Selection,
				CollideWithAreas = true,
				CollideWithBodies = false,
			};
			var result = space_state.IntersectPoint(query);

			var clickedUnits = result
				.Select(v => v["collider"].As<ClickableComponent>().unit);


			if (!Input.IsActionPressed("shift")) unitsSelected.Clear();

			var closestUnit = clickedUnits
				.OrderBy(unit => -unit.GlobalPosition.Y)
				.Where(unit => !unitsSelected.Contains(unit));
			
			if (clickedUnits.Any()) {
				unitsSelected.Add(closestUnit.First());
			}

			GD.Print($"Units selected: [{string.Join(", ", unitsSelected.Select(u => u.Name))}]");
		}
	}
}
