class_name MovementComponent extends Node2D

@export var controlled_node: Node2D
@export var SPEED = 10

var _target: Vector2

func set_target(target: Vector2) -> void:
	_target = target

func is_target_reached() -> bool:
	return controlled_node.global_position == _target

func _ready() -> void:
	assert(
		controlled_node != null,
		"MovementComponent requires a controlled node to work",
	)
	_target = controlled_node.global_position

func _physics_process(delta: float) -> void:
	var next_pos := controlled_node.global_position \
		.move_toward(_target, SPEED * delta)
	controlled_node.global_position = next_pos
