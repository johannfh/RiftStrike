class_name SelectionBox extends Area2D

@onready var collision_shape: CollisionShape2D = $CollisionShape2D
@onready var panel: Panel = $Panel

var start := Vector2.ZERO
var end := Vector2.ZERO

var active: bool = false:
	set(v):
		active = v
		if active:
			print("selection start at %v" % start)
		else:
			print("selection end at %v" % end)
	get:
		return active

func _physics_process(delta: float) -> void:
	var mouse_pos := get_global_mouse_position()
	
	if Input.is_action_just_pressed("left_click"):
		start = mouse_pos
		active = true
	if Input.is_action_just_released("left_click"):
		active = false
	
	panel.visible = active
	collision_shape.disabled = !active

	if active:
		end = get_global_mouse_position()
	
	var tl := start.min(end)
	var br := start.max(end)
	var size = br - tl
	
	if active:
		panel.size = size
		(collision_shape.shape as RectangleShape2D).size = size
		
		panel.position = tl
		collision_shape.position = tl + size * 0.5
	else:
		(collision_shape.shape as RectangleShape2D).size = Vector2.ZERO
		panel.size = Vector2.ZERO
