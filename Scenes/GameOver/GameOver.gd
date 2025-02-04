extends Control

# TODO: Cannot preload scene. Circular dependency in preload?
var TITLE_SCREEN = load("res://Scenes/TitleScreen/title_screen.tscn")

func _on_home_button_pressed() -> void:
	get_tree().change_scene_to_packed(TITLE_SCREEN)
