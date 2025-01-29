class_name MovementCommand
extends Command

var target: Vector2

func _init(_target: Vector2) -> void:
	target = _target

func _to_string() -> String:
	return "MovementCommand to (%.1f, %.1f)" % [target.x, target.y]
