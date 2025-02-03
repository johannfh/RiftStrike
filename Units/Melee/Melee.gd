class_name Melee extends Unit

func _ready() -> void:
	supported_commands = ["MovementCommand", "AttackCommand"]
