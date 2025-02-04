class_name Unit extends Node2D

func get_unit_type() -> String:
	assert(false, "Missing Implementation")
	return "unit"

var commands: Array[Command]
var selectable_component: SelectableComponent
var commands_component: CommandsComponent
