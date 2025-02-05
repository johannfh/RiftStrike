class_name MovementComponent extends Node2D

@export var controlled_node: CharacterBody2D
@export var SPEED = 10

var target: Vector2

func set_target(t: Vector2) -> void:
	target = t

func is_target_reached() -> bool:
	return controlled_node.global_position == target

func _ready() -> void:
	assert(
		controlled_node != null,
		"MovementComponent requires a controlled node to work",
	)
	target = controlled_node.global_position

func _physics_process(delta: float) -> void:
	var next_pos := controlled_node.global_position \
		.move_toward(target, SPEED * delta)
	controlled_node.global_position = next_pos
