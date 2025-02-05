extends Node2D

const GAME_OVER = preload("res://Scenes/GameOver/game_over.tscn")

@onready var unit_manager: UnitManager = $UnitManager
@onready var unit_control: UnitControl = $UnitControl
