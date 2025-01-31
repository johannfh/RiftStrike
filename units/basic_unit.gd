class_name BasicUnit
extends Unit

@export var speed: float = 70
@onready var navigation_agent: NavigationAgent2D = $NavigationAgent

func _ready() -> void:
	navigation_agent.path_desired_distance = 2.0
	navigation_agent.target_desired_distance = 2.0
	navigation_agent.debug_enabled = true

func _physics_process(delta: float) -> void:
	if len(commands) > 0:
		var command = commands[0]
		if command is MovementCommand:
			if navigation_agent.target_position != command.target:
				navigation_agent.target_position = command.target
			if navigation_agent.is_navigation_finished():
				print("reached target %v!" % position)
				commands.remove_at(0)
	
	var current_agent_position: Vector2 = position
	var next_path_position: Vector2 = navigation_agent.get_next_path_position()
	
	var direction = current_agent_position.direction_to(next_path_position)
	
	velocity = direction * speed
	
	if not navigation_agent.is_navigation_finished():
		move_and_slide()
