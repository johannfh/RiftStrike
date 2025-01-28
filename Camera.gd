extends Camera2D

var mousePos = Vector2()
var mousePosGlobal = Vector2()

var start = Vector2()
var startV = Vector2()
var end = Vector2()
var endV = Vector2()

var isDragging = false

signal area_selected
signal start_move_selection

@onready var box = get_node("../Panel")

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("LeftClick"):
		start = mousePosGlobal
		startV = mousePos
		isDragging = true
	if isDragging:
		end = mousePosGlobal
		endV = mousePos
		draw_area()
	if Input.is_action_just_released("LeftClick"):
		if startV.distance_to(mousePos) > 20:
			end = mousePosGlobal
			endV = mousePos
			isDragging = false
			draw_area(false)
			emit_signal("area_selected")
		else:
			end = start
			isDragging = false

func _input(event: InputEvent) -> void:
	if event is InputEventMouse:
		mousePos = event.position
		mousePosGlobal = get_global_mouse_position()

func draw_area(show=true):
	box.size = Vector2(abs(startV.x-endV.x), abs(startV.y-endV.y))
	box.position = Vector2(min(startV.x, endV.x), min(startV.y, endV.y))
	box.size *= int(show) # 1 if show is true, 0 if show is false
	
	
