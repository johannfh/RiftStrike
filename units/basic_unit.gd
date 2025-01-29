class_name BasicUnit
extends Unit

@export var speed: float = 50

func _physics_process(delta: float) -> void:
	if len(commands) > 0:
		var command = commands[0]
		if command is MovementCommand:
			if position.distance_to(command.target) > 1:
				position += position.direction_to(command.target) * speed * delta
			else:
				commands.remove_at(0)
