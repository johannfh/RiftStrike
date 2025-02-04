class_name Sniper extends Unit

@onready var sprite: Sprite2D = $Sprite2D
@onready var pathfind_component: PathfindComponent = $PathfindComponent

@onready var panel_selected: Panel = $PanelSelected

func get_unit_type() -> String:
	return "sniper"

func _ready() -> void:
	selectable_component = $SelectableComponent
	commands_component = $CommandsComponent

func _process(_delta: float) -> void:
	panel_selected.visible = selectable_component.selected

#func _physics_process(_delta: float) -> void:
	#var commands = selectable_component.commands
	#var cmd: Command = null
	#if len(commands) > 0:
		#cmd = commands[0]
	#
	#if cmd:
		#if cmd is MovementCommand:
			#if not pathfind_component.is_path_target(cmd.target):
				#pathfind_component.make_path(cmd.target)
			#if pathfind_component.is_target_reached():
				#commands.remove_at(0)
		#else:
			#printerr("Unknown command \"%s\"." % cmd)
			#commands.remove_at(0)
