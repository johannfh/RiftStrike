class_name ShockTrooper extends Unit

#@onready var sprite: Sprite2D = $Sprite2D
#@onready var pathfind_component: PathfindComponent = $PathfindComponent
#@onready var panel_selected: Panel = $PanelSelected

var commands: Array[Command] = []

func get_unit_type() -> String:
	return "shock_trooper"

func _ready() -> void:
	selectable_component = $SelectableComponent
	commands_component = $CommandsComponent

func _physics_process(delta: float) -> void:
	var command: Command = null
	if len(commands) > 0:
		command = commands[0]
	
	if command:
		var done := handle_command(command)
		if done:
			var cmd: Command = commands.pop_front() as Command
			print("%s finished executing %s" % [name, cmd])

func handle_command(cmd: Command) -> bool:
	if cmd is MoveCommand:
		return handle_move_command(cmd)
	return true

func handle_move_command(cmd: MoveCommand) -> bool:
	return false

func _on_command(cmd: Command, append: bool) -> void:
	if append:
		commands.append(cmd)
	else:
		commands = [cmd]
	
	print(commands)
