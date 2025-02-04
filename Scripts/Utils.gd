extends Node

enum SelectionState {
	NotSelected,
	Hovering,
	Selected,
}

func get_units() -> Array[Node2D]:
	var units: Array[Node2D]
	units.assign(get_tree().get_nodes_in_group("units"))
	return units

func get_selectables() -> Array[SelectableComponent]:
	var selectable_components: Array[SelectableComponent]
	selectable_components.assign(get_tree() \
		.get_nodes_in_group("selectable_components"))
	return selectable_components

func filter_selected(s: SelectableComponent) -> bool:
	return s.state == SelectionState.Selected
