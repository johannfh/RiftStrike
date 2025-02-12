using System.Linq;
using Godot;
using Riftstrike.units;

namespace Riftstrike.components {
	public partial class SelectableComponent : Area2D {
		[Export] public Unit unit;
		public bool IsHovered {
			get {
				var units = GetWorld2D().DirectSpaceState
				.IntersectPoint(new() {
					Position = GetGlobalMousePosition(),
					CollideWithAreas = true,
					CollideWithBodies = false,
					CollisionMask = (uint)Riftstrike.CollisionLayer.Selectable,
				})
				.Select(v => v["collider"].As<SelectableComponent>().unit)
				.OrderBy(u => -u.GlobalPosition.Y);
				var overlappingSelectors = GetOverlappingAreas();
				return (units.Any() && units.First() == unit) || overlappingSelectors.Any();
			}
		}
		public bool IsSelected
			=> UnitSelectionManager.Instance.unitsSelected.Contains(unit);
	}
}
