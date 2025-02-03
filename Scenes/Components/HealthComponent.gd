class_name HealthComponent extends Node2D

@export var MAX_HEALTH := 10.0
var health: float

func _ready() -> void:
	health = MAX_HEALTH

func damage(attack: float) -> void:
	health -= attack
