class_name Unit extends Node2D

var commands: Array[Command]
var selectable_component: SelectableComponent
var commands_component: CommandsComponent

func is_selected() -> bool:
	return selectable_component.state == Utils.SelectionState.Selected
