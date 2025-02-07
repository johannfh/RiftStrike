using Godot;
using Riftstrike.scripts.components;

namespace Riftstrike.scripts.units {
	[GlobalClass]
	public partial class ShockTrooper : Unit, ICommandable, ISelectable {
		public CommandComponent GetCommandComponent()
			=> GetNode<CommandComponent>("CommandComponent");

		public SelectionComponent GetSelectionComponent()
			=> GetNode<SelectionComponent>("SelectionComponent");

		private Panel selectionBox;

		public override void _Ready() {
			GD.Print("ShockTrooper reporting!");
			selectionBox = GetNode<Panel>("SelectionBox");
			selectionBox.Visible = false;
			GetSelectionComponent().SelectionChanged += value => selectionBox.Visible = value;
		}
	}
}
