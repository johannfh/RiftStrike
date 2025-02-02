extends Control

const WORLD = preload("res://Scenes/World/World.tscn")

func _on_start_button_pressed() -> void:
	get_tree().change_scene_to_packed(WORLD)
