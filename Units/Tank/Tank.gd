class_name Tank extends Unit

func _ready() -> void:
	selectable_component = $SelectableComponent
	commands_component = $CommandsComponent
