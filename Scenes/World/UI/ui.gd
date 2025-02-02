class_name UI
extends CanvasLayer

@onready var unit_container: HBoxContainer = $UnitContainer

# NOTE: Typed Dictionaries will be added in Godot v4.4
# See also: https://github.com/godotengine/godot-proposals/issues/56
var units: Dictionary
const UNIT_DISPLAY = preload("res://Scenes/World/UI/unit_display.tscn")

static func new_unit_display(unit: Unit) -> UnitDisplay:
	var unit_display: UnitDisplay = UNIT_DISPLAY.instantiate() as UnitDisplay
	unit_display.unit = unit
	return unit_display

func get_units() -> Array[Unit]:
	var uts: Array[Unit]
	uts.assign(get_tree().get_nodes_in_group("units"))
	return uts

func _process(_delta: float) -> void:
	for unit in get_units():
		if not units.has(unit):
			var unit_display = new_unit_display(unit)
			unit_container.add_child(unit_display)
			units[unit] = unit_display
			
			unit_display.unit = unit
			print("added unit %s to ui" % unit.name)
	
	if Input.is_action_just_pressed("select_all_units"):
		select_all_units()
	else:
		var unit_index_dict: Dictionary = {
			0: Input.is_action_just_pressed("number_1"),
			1: Input.is_action_just_pressed("number_2"),
			2: Input.is_action_just_pressed("number_3"),
			3: Input.is_action_just_pressed("number_4"),
		}

		var unit_indices: Array[int]
		
		for key in unit_index_dict.keys():
			if unit_index_dict[key]:
				unit_indices.append(key)
		
		if len(unit_indices) > 0:
			select_units(unit_indices)

func select_all_units():
	var uts: Array[Unit]
	uts.assign(get_tree().get_nodes_in_group("units"))
	for u in uts:
		u.selected = true

func select_units(indices: Array[int]) -> void:
	var uts: Array[Unit]
	uts.assign(get_tree().get_nodes_in_group("units"))
	for idx in len(uts):
		uts[idx].selected = indices.has(idx)
