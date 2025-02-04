class_name UnitManager extends Node

var _selected_units: Array[Unit]

func _ready() -> void:
	for u in get_units():
		_register_unit(u)

func add_unit(u: Unit) -> void:
	add_child(u)
	_register_unit(u)

func _register_unit(u: Unit) -> void:
	if u.selectable_component:
		# connect commands to unit (e.g. bind command key to unit.execute)
		u.selectable_component.selection_changed.connect(
			_on_unit_selection_changed
		)

func get_units() -> Array[Unit]:
	var units: Array[Unit]
	units.assign(get_children())
	return units

func get_selected_units() -> Array[Unit]:
	return _selected_units

func _on_unit_selection_changed(unit: Unit, selected: bool):
	print("selection changed %s (%s)" % [unit.name, selected])
	if selected:
		if not unit in _selected_units:
			_selected_units.append(unit)
	else:
		_selected_units = _selected_units \
			.filter(func(u: Unit): return u != unit)
	
	print(len(_selected_units))

static func is_commandable(u: Unit) -> bool:
	return u.commands_component != null

static func is_selectable(u: Unit) -> bool:
	return u.selectable_component != null

static func is_selected(u: Unit) -> bool:
	return is_selectable(u) and \
		u.selectable_component.selected
