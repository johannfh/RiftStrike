extends Node2D

const GAME_OVER = preload("res://Scenes/GameOver/GameOver.tscn")

@onready var unit_manager: UnitManager = $UnitManager
@onready var unit_control: UnitControl = $UnitControl

func _ready() -> void:
	for i in range(3):
		var sniper := UnitFactory.create_sniper()
		unit_manager.add_unit(sniper)
