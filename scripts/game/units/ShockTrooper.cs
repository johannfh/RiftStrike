using Godot;
using Riftstrike.scripts.game.components;

namespace Riftstrike.scripts.game.units {
	[GlobalClass]
	public partial class ShockTrooper : Unit, ICommandable, ISelectable {
		private CommandComponent commandComponent;
		private SelectionComponent selectionComponent;

		public override void _Ready() {
			commandComponent = GetNode<CommandComponent>("CommandComponent");
			selectionComponent = GetNode<SelectionComponent>("SelectionComponent");
		}

		public CommandComponent GetCommandComponent()
			=> commandComponent;

		public SelectionComponent GetSelectionComponent()
			=> selectionComponent;
	}
}
