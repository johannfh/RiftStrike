extends Camera2D

@export_range(10, 500, 1) var camera_move_speed = 150

signal move_command(target: Vector2, append: bool)

func _process(delta: float) -> void:
	var direction = Vector2.ZERO
	
	direction.x = Input.get_axis("camera_left", "camera_right")
	direction.y = Input.get_axis("camera_up", "camera_down")
	
	direction = direction.normalized()
	position += direction * delta * camera_move_speed
	
	if Input.is_action_just_pressed("right_click"):
		var appendCommand = Input.is_action_pressed("shift")
		var mouseGlobalPos = get_viewport().get_final_transform().basis_xform_inv(get_global_mouse_position())
		move_command.emit(mouseGlobalPos, appendCommand)
