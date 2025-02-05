class_name Unit extends CharacterBody2D

func get_unit_type() -> String:
	assert(false, "Missing Implementation")
	return "unit"

var selectable_component: SelectableComponent
var commands_component: CommandsComponent
