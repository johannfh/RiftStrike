class_name SelectionBox extends Area2D

@export var unit_manager: UnitManager

@onready var panel: Panel = $Panel
@onready var collision_shape: CollisionShape2D = $CollisionShape2D

var start := Vector2.ZERO
var end := Vector2.ZERO

var _active: bool = false

func _process(delta: float) -> void:
	var mouse_pos := get_global_mouse_position()
	
	if Input.is_action_just_pressed("left_click"):
		start = mouse_pos
		_active = true
		print("selection start at %v" % start)
	if Input.is_action_just_released("left_click"):
		_active = false
		print("selection end at %v" % end)
	
		# WARNING: This assumes that only SelectableComponent
		# areas can collide (as decided on by the physics layers/masks)
		var new_selected: Array[Unit]
		new_selected.assign(get_overlapping_areas() \
			.map(func(s: SelectableComponent): return s.parent))
		
		for u in new_selected:
			u.selectable_component.selected = true
		
		if not Input.is_action_pressed("shift"):
			for u in unit_manager.get_selected_units():
				if u not in new_selected:
					u.selectable_component.selected = false
	
	panel.visible = _active
	collision_shape.disabled = not _active

func _physics_process(delta: float) -> void:
	

	if _active:
		end = get_global_mouse_position()
	
	var tl := start.min(end)
	var br := start.max(end)
	var size = br - tl
	
	if _active:
		panel.size = size
		(collision_shape.shape as RectangleShape2D).size = size
		
		panel.position = tl
		collision_shape.position = tl + size * 0.5
	else:
		(collision_shape.shape as RectangleShape2D).size = Vector2.ZERO
		panel.size = Vector2.ZERO
