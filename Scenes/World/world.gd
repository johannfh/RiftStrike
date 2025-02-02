extends Node2D

const GAME_OVER = preload("res://Scenes/GameOver/GameOver.tscn")

func _on_move_command(target: Vector2, append: bool) -> void:
	print("move units to %v" % target)
	var units: Array[Unit]
	units.assign(get_tree().get_nodes_in_group("units"))
	var selected_units: Array[Unit] = units \
		.filter(func(u: Unit): return u.selected)
	
	for unit in selected_units:
		var cmd = MovementCommand.new(target)
		if append:
			unit.commands.append(cmd)
		else:
			unit.commands = [cmd]
		print("commands %s" % str(unit.commands))

func _process(_delta: float) -> void:
	var units: Array[Unit]
	units.assign(get_tree().get_nodes_in_group("units"))
	var units_alive = units.filter(func(u: Unit): return u.hp > 0)
	if len(units_alive) <= 0:
		get_tree().change_scene_to_packed(GAME_OVER)
