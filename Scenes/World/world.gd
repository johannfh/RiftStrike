extends Node2D

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
