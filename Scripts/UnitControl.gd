class_name UnitControl extends Control

@export var selection_box: SelectionBox
@export var unit_manager: UnitManager
var commands: Dictionary = {}

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
		var cmd := MovementCommand.new(Vector2.ZERO)
		var units: Array[Unit] = unit_manager.get_units() \
			# filter selected units
			.filter(func(u: Unit): return u.selectable_component.selected) \
			# filter units supporting the command
			.filter(func(u: Unit): return cmd.type in u.commands_component.supported)
		
		print("move %d units" % len(units))
		
		# TODO: only on matching unit context or global command
		
		for u in units:
			u.commands.append(cmd)
	
	#if Input.is_action_just_pressed("escape"):
	#	for s in unit_manager.get_selected_units():
	#		s.state = Utils.SelectionState.NotSelected

func create_input_dict() -> Dictionary:
	var dict: Dictionary = {}
	
	var units := unit_manager \
		.get_units() \
		.filter(UnitManager.is_selected)
	
	return dict

# marine a -> attack
# 	then attack per marine
# sniper q -> snipecmd
# marine q -> stim
