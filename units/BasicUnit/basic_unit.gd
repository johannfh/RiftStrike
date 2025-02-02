class_name BasicUnit
extends Unit

@onready var nav_agent: NavigationAgent2D = $NavigationAgent
@onready var selected_circle: Panel = $SelectedCircle

@export var speed: float = 120

func get_speed() -> float:
	var multiplier = 1
	for upgrade in upgrades:
		if upgrade is UnitUpgradeMoveSpeed:
			multiplier += upgrade.multiplier
	return speed * multiplier

func _ready() -> void:
	nav_agent.path_desired_distance = 2.0
	nav_agent.target_desired_distance = 2.0
	make_path(position)

func _process(_delta: float) -> void:
	selected_circle.visible = selected

func _physics_process(_delta: float) -> void:
	# Pull in commands
	if len(commands) > 0:
		var command = commands[0]
		if command is MovementCommand:
			if nav_agent.target_position != command.target:
				make_path(command.target)
			if nav_agent.is_navigation_finished():
				print("%s reached target %v!" % [name, position])
				commands.remove_at(0)
	
	call_deferred("handle_movement")

func handle_movement() -> void:
	# NOTE: waiting until before next frame to have nav map initialized
	await get_tree().physics_frame
	var next_path_pos = nav_agent.get_next_path_position()
	var direction = global_position.direction_to(next_path_pos)
	velocity = direction * get_speed()
	
	if not nav_agent.is_navigation_finished():
		move_and_slide()

func make_path(pos: Vector2):
	nav_agent.target_position = pos

func takes_upgrade(upgrade: UnitUpgrade):
	return upgrade is UnitUpgradeMoveSpeed
