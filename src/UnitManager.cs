using System.Linq;
using Godot;
using Godot.Collections;
using Riftstrike.components;
using Riftstrike.units;

namespace Riftstrike
{
	[GlobalClass]
	public partial class UnitManager : Node2D
	{
		// TODO: Create Instance when none is found?
		// Also maybe not because it might not be intended that way.
		// (More per-scene control)
		public static UnitManager Instance { get; private set; }

		// The Timer instance specifies the max duration for a mouse down
		// to count as a click. Longer actions will count as things like Dragging.
		// TODO: Expose a signal that is connected to SelectUnitTimer.Timeout
		// to hide the internals (private Timer; [Signal] public delegate).
		[Export] private double dragThreshold = 5;
		private Vector2 dragStartPos = Vector2.Zero;
		private Vector2 dragEndPos = Vector2.Zero;
		private bool IsDragging = false;
		[Signal] public delegate void DragStartEventHandler();

		public override void _Ready()
		{
			base._Ready();
			if (Instance != null && Instance != this)
			{
				QueueFree();
				return;
			}
			Instance = this;
			DragStart += () => GD.Print("Drag start");
			GD.Print($"Initialized {nameof(UnitManager)}");
			GD.Print($"Units: [{string.Join(", ", units.Select(u => u.Name))}]");
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			if (Instance == this) Instance = null;
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
			var mousePos = GetGlobalMousePosition();
			if (Input.IsActionJustPressed("left_click"))
			{
				dragStartPos = mousePos;
			}
			dragEndPos = mousePos;
			if (!IsDragging && Input.IsActionPressed("left_click") && dragStartPos.DistanceTo(dragEndPos) > dragThreshold)
			{
				IsDragging = true;
				EmitSignal(SignalName.DragStart);
			}
			if (Input.IsActionJustReleased("left_click"))
			{
				if (!IsDragging) HandleSelectUnit();
				else IsDragging = false;
			}

			if (Input.IsActionJustPressed("right_click"))
			{
				var append = Input.IsActionPressed("shift");
				unitsSelected.OfType<IWalk>()
					.ForEach(walkable => walkable.WalkTo(mousePos, append));
			}

			var space_state = GetWorld2D().DirectSpaceState;
			var query = new PhysicsPointQueryParameters2D
			{
				Position = mousePos,
				CollisionMask = (uint)CollisionLayer.Selectable,
				CollideWithAreas = true,
				CollideWithBodies = false,
			};
			var result = space_state.IntersectPoint(query);
			if (result.Any())
			{
				CursorSettings.Instance.Cursor = Cursor.Select;
			}
			else
			{
				CursorSettings.Instance.Cursor = Cursor.Default;
			}
		}

		public readonly Array<Unit> units = new();
		public readonly Array<Unit> unitsSelected = new();

		public void HandleSelectUnit()
		{
			var mousePos = GetGlobalMousePosition();
			GD.Print($"Select unit at {mousePos}");

			var space_state = GetWorld2D().DirectSpaceState;
			var query = new PhysicsPointQueryParameters2D
			{
				Position = mousePos,
				CollisionMask = (uint)CollisionLayer.Selectable,
				CollideWithAreas = true,
				CollideWithBodies = false,
			};
			var result = space_state.IntersectPoint(query);

			var clickedUnits = result
				.Select(v => v["collider"].As<SelectableComponent>().unit);


			if (!Input.IsActionPressed("shift")) unitsSelected.Clear();

			var closestUnit = clickedUnits
				.OrderBy(unit => -unit.GlobalPosition.Y)
				.Where(unit => !unitsSelected.Contains(unit));

			if (clickedUnits.Any())
			{
				unitsSelected.Add(closestUnit.First());
			}

			GD.Print($"Units selected: [{string.Join(", ", unitsSelected.Select(u => u.Name))}]");
		}
	}
}
