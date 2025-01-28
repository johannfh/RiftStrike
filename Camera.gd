extends Camera2D

@export_range(10, 500, 1) var camera_move_speed = 150

signal move_command(position: Vector2)

func _process(delta: float) -> void:
	var direction = Vector2.ZERO
	
	direction.x = Input.get_axis("camera_left", "camera_right")
	direction.y = Input.get_axis("camera_up", "camera_down")
	
	direction = direction.normalized()
	position += direction * delta * camera_move_speed
	
	if Input.is_action_just_pressed("cursor_action_secondary"):
		move_command.emit(get_viewport().get_final_transform().basis_xform_inv(get_global_mouse_position()))


func _on_move_command(target: Vector2) -> void:
	print("move units to %v" % target)
	var units: Array[Node2D]
	units.assign(get_tree().get_nodes_in_group("units"))
	
