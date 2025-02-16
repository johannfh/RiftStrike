using Godot;

namespace Riftstrike.units
{
	[GlobalClass]
	public abstract partial class Unit : Node2D
	{
		[Export(PropertyHint.None, "suffix:pixels")]
		public double SafeDistance = 300;

		public override void _Ready()
		{
			base._Ready();
			UnitManager.Instance.units.Add(this);
		}

		public override void _ExitTree()
		{
			UnitManager.Instance.units.Remove(this);
			UnitManager.Instance.unitsSelected.Remove(this);
			base._ExitTree();
		}
	}

	public interface IWalk
	{
		public void WalkTo(Vector2 targetPosition, bool append);
	}
}
