class_name SelectableComponent extends Area2D

@export var parent: Unit

signal selection_changed(unit: Unit, selected: bool)

var selected: bool = false:
	set(v):
		# when new state is different
		if selected != v:
			selection_changed.emit(parent, v)
		selected = v
	get:
		return selected
