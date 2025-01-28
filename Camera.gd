extends Camera2D

@export_range(10, 500, 1) var camera_move_speed = 150

func _process(delta: float) -> void:
	var direction = Vector2.ZERO
	
	direction.x = Input.get_axis("camera_left", "camera_right")
	direction.y = Input.get_axis("camera_up", "camera_down")
	
	direction = direction.normalized()
	position += direction * delta * camera_move_speed
