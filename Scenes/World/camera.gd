extends Camera2D

@export_range(10, 500, 1) var camera_move_speed = 150
@export_range(0.005, 0.5, 0.001) var zoom_sensitivity = 0.005

const MIN_ZOOM = Vector2(0.1, 0.1)
const MAX_ZOOM = Vector2(1, 1)

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
	
	var zoom_in = Input.is_action_just_released("zoom_in")
	var zoom_out = Input.is_action_just_released("zoom_out")
	
	var zoom_direction = int(zoom_in) - int(zoom_out)
	
	if zoom_direction != 0:
		if zoom_direction == 1:
			print("Zoom in")
		if zoom_direction == -1:
			print("Zoom out")
		zoom.x += zoom_direction * zoom_sensitivity
		zoom.y += zoom_direction * zoom_sensitivity
		zoom = zoom.clamp(MIN_ZOOM, MAX_ZOOM)
