using System.Linq;
using Riftstrike.src.units;

namespace Riftstrike.components
{
	[GlobalClass]
	public partial class SelectableComponent : Area2D
	{
		[Export]
		public Unit unit;

		public bool IsHovered
		{
			get
			{
				var units = GetWorld2D().DirectSpaceState
				.IntersectPoint(new()
				{
					Position = GetGlobalMousePosition(),
					CollideWithAreas = true,
					CollideWithBodies = false,
					CollisionMask = (uint)Riftstrike.CollisionLayer.Selectable,
				})
				.Select(v => v["collider"].As<SelectableComponent>().unit)
				.OrderBy(u => -u.GlobalPosition.Y);
				var overlappingSelectors = GetOverlappingAreas();
				return (units.Any() && units.First() == unit) || overlappingSelectors.Count != 0;
			}
		}
		public bool IsSelected
			=> UnitManager.Instance.unitsSelected.Contains(unit);
	}
}
