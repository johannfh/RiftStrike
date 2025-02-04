class_name MoveCommand extends Command

var target: Vector2

func _init(_target: Vector2) -> void:
	target = _target
	type = Command.Type.Move

func _to_string() -> String:
	return "MoveCommand"
