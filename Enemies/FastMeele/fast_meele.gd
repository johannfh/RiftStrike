class_name FastMeeleEnemy
extends Enemy

var speed: float = 200
var damage: float = 1

@onready var attack_cooldown: Timer = $AttackCooldown
@onready var recalc_target_timer: Timer = $RecalculateTargetTimer
@onready var sprite: AnimatedSprite2D = $Sprite
@onready var state_machine: StateMachine = $StateMachine
@onready var nav_agent: NavigationAgent2D = $NavigationAgent


func _ready() -> void:
	health_component = $HealthComponent

func _process(delta: float) -> void:
	state_machine.current_state.update(delta)
	if health_component.health > 0:
		sprite.play("alive")
	else:
		sprite.play("dead")

func _physics_process(delta: float) -> void:
	state_machine.current_state.physics_update(delta)
	
	# calculate movement towards nexts navigation path position
	var next_path_pos = nav_agent.get_next_path_position()
	var direction = position.direction_to(next_path_pos)
	velocity = direction * speed
	
	# idle if target is reached
	if nav_agent.is_navigation_finished():
		velocity = Vector2.ZERO
	
	move_and_slide()
	
func _on_attack_cooldown_timeout() -> void:
	state_machine.current_state \
		.transitioned.emit(state_machine.current_state, "attacking")
