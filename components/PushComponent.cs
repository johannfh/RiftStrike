using Godot;
using System.Linq;

namespace Riftstrike.components {
	public partial class PushComponent : Area2D {
		public Vector2 PushDirection {
			get {
				var overlapping = GetOverlappingAreas()
					.Where(v => v != this);
				
				var averageDirection = overlapping
					.Select(v => v.GlobalPosition.DirectionTo(GlobalPosition))
					.AverageVector()
					.Normalized();
				
				return overlapping.Any() && averageDirection.Length() == 0
					? new(GD.Randf() + 1, GD.Randf() + 1)
					: averageDirection;
			}
		}
	}
}
