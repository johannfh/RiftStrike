class_name UnitControl extends Control

@export var unit_manager: UnitManager
var commands: Dictionary = {}

signal selection_changed

# Marine:
#	Move m
#	Attack a
#	Stop s
#	Stim t

# Sniper:
#	Move m
#	Attack
#	Stop s
#	Snipe q
#	Cloak c

# Tank:
#	Move m
#	Attack a
#	Stop s
#	ToggleSiege q
#	Cloak c

# [Marine, Marine, Sniper, Tank, Tank]

# Scenario 1:
# user input: press "m"
# -> for each unit supporting "m"
# -> execute command mapping to "m"

func _process(delta: float) -> void:
	# Move Command trigger
	if Input.is_action_just_pressed("right_click"):
		var cmd := GlobalMovementCommand.new(Vector2.ZERO)
		var units: Array[Unit] = unit_manager.get_units() \
			# filter selected units
			.filter(func(u: Unit): return u.is_selected()) \
			# filter units supporting the command
			.filter(func(u: Unit): return cmd.type in u.commands_component.supported)
		
		print("move %d units" % len(units))
		
		# TODO: only on matching unit context or global command
		
		for u in units:
			u.commands.append(cmd)

func create_input_dict() -> Dictionary:
	var dict: Dictionary = {}
	
	var units := unit_manager.get_units()
	var u_map := Utils.units_by_type(units)
	
	return u_map

# marine a -> attack
# 	then attack per marine
# sniper q -> snipecmd
# marine q -> stim


func execute_command(cmd: Command) -> void:
	var name := Utils.get_typename(cmd)
	for unit in unit_manager.get_units():
		# TODO: filter only selected units
		if name in unit.supported_commands:
			unit.commands.append(cmd)
			pass


func _on_selection_changed() -> void:
	pass
	# recalculate input mappings
	var mapping: Dictionary = {
		"GlobalMovementCommand": 1
	}
