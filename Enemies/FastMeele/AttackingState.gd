class_name FastMeeleAttackingState extends State

var target: Unit
@onready var hitbox_component: HitboxComponent = $"../../HitboxComponent"
@onready var root: FastMeeleEnemy = $"../.."
@onready var nav_agent: NavigationAgent2D = $"../../NavigationAgent"
@onready var attack_cooldown: Timer = $"../../AttackCooldown"

func exit():
	attack_cooldown.start()

func physics_update(delta: float) -> void:
	if target:
		nav_agent.target_position = target.position
	var overlapping_nodes := hitbox_component \
		.get_overlapping_areas() \
		.filter(func(n: Node2D): return n is HitboxComponent)
		
	var overlapping_hitboxes: Array[HitboxComponent]
	overlapping_hitboxes.assign(overlapping_nodes)
	
	var closest: HitboxComponent = null
	var closest_dist := INF
	for hitbox in overlapping_hitboxes:
		var dist := root.position.distance_to(hitbox.position)
		if dist < closest_dist:
			closest = hitbox
			closest_dist = dist
	
	if closest:
		print("attacked!")
		closest.damage(root.damage)
		transitioned.emit(self, "idle")

func _on_recalculate_target_timer_timeout() -> void:
	recalculate_target()
	
func recalculate_target() -> void:
	var units = get_units()
	target = get_closest_to(root.position, units)

func get_units() -> Array[Unit]:
	var units: Array[Unit]
	units.assign(get_tree().get_nodes_in_group("units"))
	return units

func get_closest_to(pos: Vector2, units: Array[Unit]) -> Unit:
	var unit: Unit = null
	var closest_dist: float = 0
	
	for u in units:
		if u.health_component.health <= 0:
			continue
		var dist = u.position.distance_to(pos)
		if not unit or dist < closest_dist:
			closest_dist = dist
			unit = u
	
	return unit
