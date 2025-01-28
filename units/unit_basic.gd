extends Unit

var target = Vector2()
enum Action { Walk, Attack }
var action = Action.Walk

func walk_towards(destination: Vector2, append: bool):
	target = destination
	action = Action.Walk

func _ready() -> void:
	target = position

func _process(delta: float) -> void:
	pass
