class_name GlobalMovementCommand extends Command

var target: Vector2

func _init(_target: Vector2) -> void:
	target = _target
	type = CommandsComponent.CommandTypes.Movement

func _to_string() -> String:
	return "GlobalMovementCommand"
