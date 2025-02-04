class_name UnitControl extends Control

@export var unit_manager: UnitManager
@onready var selection_box: SelectionBox = $SelectionBox

func _ready() -> void:
	unit_manager.selected_units_changed \
		.connect(_on_unit_selection_changed)
	selection_box.unit_manager = unit_manager

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

#region Input Mappings Logic
var active_keymaps: Array[KeyMap]
var unit_types: Dictionary
var current_unit_type: String = ""
var current_unit_supported_commands: Array[Command.Type] = []
var input_mappings: Dictionary = {}

# regenerate input mappings, when the unit selection changes
func _on_unit_selection_changed(selected_units: Array[Unit]):
	input_mappings = generate_input_mappings(selected_units)

func generate_input_mappings(selected_units: Array[Unit]) -> Dictionary:
	var result = {}
	
	# get empty case out of the way
	if len(selected_units) == 0:
		current_unit_type = ""
		unit_types = {}
		return result
	
	# generate a dictionary of unit types
	# mapping to their supported commands
	unit_types = {}
	for unit in selected_units:
		var type := unit.get_unit_type()
		if type not in unit_types:
			unit_types[type] = unit.commands_component.supported
	
	# when the current unit type goes missing from the selected units
	if not current_unit_type in unit_types:
		# unset the current unit type
		current_unit_type = ""
	
	# when no current unit type is set
	if current_unit_type == "":
		# fall back to the first unit type that exists
		current_unit_type = unit_types.keys()[0]
	
	# update supported commands array
	current_unit_supported_commands = unit_types[current_unit_type]
	
	# generate keymap array for supported commands
	for type in current_unit_supported_commands:
		var keymap := Command.get_keymap(type)
		result[keymap.input_action] = keymap
	
	print("selected unit types (%d): [%s]" %
		[len(unit_types), ", ".join(unit_types.keys())])
	print(
		"generated input mappings for %d units, current type: \"%s\"" %
		[len(selected_units), current_unit_type],
	)
	
	print("input mappings: %s" % result)
	return result


func select_next_unit_type():
	if len(unit_types) == 0:
		return
	
	var old_unit_type := current_unit_type
	var types := unit_types.keys()
	
	var next_idx := types.find(old_unit_type) + 1
	
	if next_idx == len(types):
		next_idx = 0
	
	current_unit_type = types[next_idx]
	
	print("Switch to next unit type in [%s] (\"%s\" -> \"%s\")" %
		[", ".join(unit_types.keys()), old_unit_type, current_unit_type])
	
	input_mappings = generate_input_mappings(
		unit_manager.get_selected_units(),
	)

func select_previous_unit_type():
	if len(unit_types) == 0:
		return
		
	var old_unit_type := current_unit_type
	var types := unit_types.keys()
	
	var next_idx := types.find(old_unit_type) - 1
	
	if next_idx < 0:
		next_idx = len(types) - 1
	
	current_unit_type = types[next_idx]
	
	print("Switch to prev unit type in [%s] (\"%s\" -> \"%s\")" %
		[", ".join(unit_types.keys()), old_unit_type, current_unit_type])
	
	input_mappings = generate_input_mappings(
		unit_manager.get_selected_units(),
	)
#endregion

#region Command Handlers
func get_handler_for_command(type: Command.Type) -> Callable:
	match type:
		Command.Type.Stop: return handle_stop_command
		Command.Type.Move: return handle_move_command
		Command.Type.Attack: return handle_attack_command
		Command.Type.Cloak: return handle_cloak_command
		Command.Type.Decloak: return handle_decloak_command
		Command.Type.Siege: return handle_siege_command
		Command.Type.Unsiege: return handle_unsiege_command
	
	return func(append: bool) -> void: \
		printerr("Empty handler for command \"%s\"" % type)

func handle_stop_command(append: bool) -> void:
	var cmd := StopCommand.new()
	dispatch_command(cmd, append)

func handle_move_command(append: bool) -> void:
	var target := get_global_mouse_position()
	var cmd := MoveCommand.new(target)
	dispatch_command(cmd, append)

func handle_attack_command(append: bool) -> void:
	var target := get_global_mouse_position()
	var cmd := AttackCommand.new(target)
	dispatch_command(cmd, append)
	
func handle_cloak_command(append: bool) -> void:
	var cmd := CloakCommand.new()
	dispatch_command(cmd, append)

func handle_decloak_command(append: bool) -> void:
	var cmd := DecloakCommand.new()
	dispatch_command(cmd, append)

func handle_siege_command(append: bool) -> void:
	var cmd := SiegeCommand.new()
	dispatch_command(cmd, append)

func handle_unsiege_command(append: bool) -> void:
	var cmd := UnsiegeCommand.new()
	dispatch_command(cmd, append)
#endregion

# TODO: Spells only first of unit type
# TODO: Cooldown
func dispatch_command(cmd: Command, append: bool) -> void:
	var units := unit_manager.get_selected_units() \
		.filter(func(u: Unit): return \
			cmd.type in u.commands_component.supported)
	print(
		"dispatching %s command to %d units (append: %s)" %
		[cmd, len(units), append],
	)
	for u in units:
		u.commands_component.command.emit(cmd, append)

#region Input Processing
# callable or null
var next_command_handler: Variant
var command_handled := false

func _process(delta: float) -> void:
	var append := Input.is_action_pressed("shift")
	
	if Input.is_action_just_pressed("right_click"):
		handle_move_command(append)
	
	var new_handlers: Dictionary = {}
	
	for key in input_mappings.keys():
		var keymap: KeyMap = input_mappings[key]
		var action = keymap.input_action
		if Input.is_action_just_pressed(action):
			new_handlers[action] = keymap.handler
	
	if len(new_handlers.keys()) > 0:
		var first_key = new_handlers.keys()[0]
		next_command_handler = new_handlers[first_key]
	
	if Input.is_action_just_pressed("left_click"):
		if next_command_handler:
			next_command_handler.call(append)
			next_command_handler = []
			command_handled = true
	
	if Input.is_action_just_released("left_click"):
		command_handled = false
	
	selection_box.update(delta)
	
	if Input.is_action_just_pressed("cycle_unit_types"):
		var backwards := Input.is_action_pressed("shift")
		if backwards:
			select_previous_unit_type()
		else:
			select_next_unit_type()
	
	#if Input.is_action_just_pressed("escape"):
	#	for s in unit_manager.get_selected_units():
	#		s.state = Utils.SelectionState.NotSelected

func _physics_process(delta: float) -> void:
	# only do selection box stuff, when no command has been handled
	selection_box.reactive = not command_handled
	selection_box.physics_update(delta)
#endregion

# marine a -> attack
# 	then attack per marine
# sniper q -> snipe
# marine t -> stim
