class_name BasicUnit
extends Unit

@export var speed: float = 5000
@onready var nav_agent: NavigationAgent2D = $NavigationAgent

func _ready() -> void:
	nav_agent.path_desired_distance = 2.0
	nav_agent.target_desired_distance = 2.0
	nav_agent.debug_enabled = true
	make_path(position)

func _physics_process(delta: float) -> void:
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
	var new_velocity = direction * speed * delta
	
	nav_agent.velocity = new_velocity


func _on_navigation_agent_velocity_computed(safe_velocity: Vector2) -> void:
	velocity = velocity.move_toward(safe_velocity, 100)
	if not nav_agent.is_navigation_finished():
		move_and_slide()

func make_path(pos: Vector2):
	nav_agent.target_position = pos
