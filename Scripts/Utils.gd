extends Node

enum SelectionState {
	NotSelected,
	Hovering,
	Selected,
}

func get_typename(obj: Object) -> String:
	return obj.get_script().resource_path.get_file().get_basename()

func get_units() -> Array[Node2D]:
	var units: Array[Node2D]
	units.assign(get_tree().get_nodes_in_group("units"))
	return units

func units_by_type(units: Array[Unit]) -> Dictionary:
	var types: Dictionary = {}
	
	for u in units:
		var t := get_typename(u)
		if not types.has(t):
			types[t] = []
		types[t].append(u)
	
	return types

func get_selectables() -> Array[SelectableComponent]:
	var selectable_components: Array[SelectableComponent]
	selectable_components.assign(get_tree() \
		.get_nodes_in_group("selectable_components"))
	return selectable_components

func filter_selected(s: SelectableComponent) -> bool:
	return s.state == SelectionState.Selected
