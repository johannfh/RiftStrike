class_name MovementCommand
extends Command

enum MoveType { Walk, Attack }

class Params:
	var target: Vector2
	var move_type: MoveType
	
	func _init(target: Vector2, move_type: MoveType = MoveType.Walk) -> void:
		self.target = target

func execute(unit: Unit, data: Object = null) -> void:
	var params = data as Params
