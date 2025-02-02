class_name BasicUnit
extends Unit

@onready var nav_agent: NavigationAgent2D = $NavigationAgent
@onready var selected_circle: Panel = $SelectedCircle
@onready var sprite: AnimatedSprite2D = $Sprite
@onready var collision_shape: CollisionShape2D = $CollisionShape
@onready var attack_range: Area2D = $"Attack Range"

@export var speed: float = 150.0
const MAX_SPEED: float = 300.0

func _ready() -> void:
	make_path(position)
	nav_agent.path_desired_distance = 2.0
	nav_agent.target_desired_distance = 2.0
	health_component = $HealthComponent

func _process(_delta: float) -> void:
	selected_circle.visible = selected and health_component.health > 0
	if health_component.health > 0:
		sprite.play("alive")
	else:
		sprite.play("dead")


func _physics_process(_delta: float) -> void:
	handle_commands()
	if health_component.health <= 0:
		commands = []
		collision_shape.disabled = true
	
	var next_path_pos = nav_agent.get_next_path_position()
	var direction = global_position.direction_to(next_path_pos)
	velocity = direction * speed
	
	if not nav_agent.is_navigation_finished() and health_component.health > 0:
		move_and_slide()

func make_path(pos: Vector2):
	nav_agent.target_position = pos

func is_current_path(pos: Vector2):
	return nav_agent.target_position == pos

func handle_commands() -> void:
	# pull new commands from queue
	var command := commands[0] if (len(commands) > 0) else null
	if not command: return
	
	if command is MovementCommand:
		var done := handle_movement_command(command)
		if done:
			commands.remove_at(0)

func handle_movement_command(cmd: MovementCommand) -> bool:
	if not is_current_path(cmd.target):
		make_path(cmd.target)
	return nav_agent.is_navigation_finished()


func _on_attack_timer_timeout() -> void:
	pass # Replace with function body.
