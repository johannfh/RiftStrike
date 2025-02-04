class_name PathfindComponent extends NavigationAgent2D

@export var movement_component: MovementComponent

func is_path_target(target: Vector2) -> bool:
	return target_position == target

func make_path(target: Vector2) -> void:
	target_position = target

func _ready() -> void:
	assert(
		movement_component != null,
		"PathfindComponent requires a MovementComponent to work."
	)

func _physics_process(_delta: float) -> void:
	var next_pos := get_next_path_position()
	movement_component.set_target(next_pos)
