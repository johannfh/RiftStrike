class_name Tank extends Unit

func get_unit_type() -> String:
	return "tank"

func _ready() -> void:
	selectable_component = $SelectableComponent
	commands_component = $CommandsComponent
