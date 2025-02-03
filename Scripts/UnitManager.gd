class_name UnitManager extends Node

func add_unit(u: Unit) -> void:
	add_child(u)
	# connect commands to unit (e.g. bind command key to unit.execute)

func get_units() -> Array[Unit]:
	var units: Array[Unit]
	units.assign(get_children())
	return units
