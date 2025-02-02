class_name FastMeeleEnemy
extends Enemy

var speed: float = 200
var damage: float = 1
var target: Unit
@onready var nav_agent: NavigationAgent2D = $NavigationAgent
@onready var attack_cooldown: Timer = $AttackCooldown
@onready var recalc_target_timer: Timer = $RecalculateTargetTimer
@onready var sprite: AnimatedSprite2D = $Sprite
var can_attack = false


func _process(_delta: float) -> void:
	if hp > 0:
		sprite.play("alive")
	else:
		sprite.play("dead")

func _physics_process(delta: float) -> void:
	if target:
		nav_agent.target_position = target.position
	
	# calculate movement towards nexts navigation path position
	var next_path_pos = nav_agent.get_next_path_position()
	var direction = global_position.direction_to(next_path_pos)
	velocity = direction * speed
	
	# idle if target is reached
	if nav_agent.is_navigation_finished():
		velocity = Vector2.ZERO
	
	var collider = move_and_collide(velocity * delta)
	if collider and collider.get_collider() is Unit:
		var unit: Unit = collider.get_collider() as Unit
		if unit.hp <= 0:
			recalculate_target()
		elif can_attack:
			var old_hp = unit.hp
			unit.hp -= damage
			attack_cooldown.start()
			can_attack = false
			print(
				"%s attacked %s and dealt %.2f damage! %s now has %.2f hp" %
				[name, unit.name, old_hp-unit.hp, unit.name, unit.hp]
			)
			# retarget collider
			target = unit

func recalculate_target() -> void:
	var units = get_units()
	target = get_closest_to(position, units)

func get_units() -> Array[Unit]:
	var units: Array[Unit]
	units.assign(get_tree().get_nodes_in_group("units"))
	return units

func get_closest_to(pos: Vector2, units: Array[Unit]) -> Unit:
	var unit: Unit = null
	var closest_dist: float = 0
	
	for u in units:
		if u.hp <= 0:
			continue
		var dist = u.position.distance_to(pos)
		if not unit or dist < closest_dist:
			closest_dist = dist
			unit = u
	
	return unit

func _on_recalculate_target_timer_timeout() -> void:
	recalculate_target()
	
func _on_attack_cooldown_timeout() -> void:
	attack_cooldown.stop()
	can_attack = true
