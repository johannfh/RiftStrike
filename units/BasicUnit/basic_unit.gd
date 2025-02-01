class_name BasicUnit
extends Unit

@export var speed: float = 120

func get_speed() -> float:
	var result = speed
	var multiplier = 1
	# TODO: count multiplier
	for upgrade in upgrades:
		if upgrade is UnitUpgradeMoveSpeed:
			result *= upgrade.multiplier
	return result

@onready var nav_agent: NavigationAgent2D = $NavigationAgent

func _ready() -> void:
	nav_agent.path_desired_distance = 2.0
	nav_agent.target_desired_distance = 2.0
	make_path(position)

func _physics_process(_delta: float) -> void:
	# Pull in commands
	if len(commands) > 0:
		var command = commands[0]
		if command is MovementCommand:
			if nav_agent.target_position != command.target:
				make_path(command.target)
			if nav_agent.is_navigation_finished():
				print("reached target %v!" % position)
				commands.remove_at(0)
	var next_path_pos = nav_agent.get_next_path_position()
	var direction = global_position.direction_to(next_path_pos)
	velocity = direction * get_speed()
	
	if not nav_agent.is_navigation_finished():
		move_and_slide()

func make_path(pos: Vector2):
	nav_agent.target_position = pos

func takes_upgrade(upgrade: UnitUpgrade):
	return upgrade is UnitUpgradeMoveSpeed
