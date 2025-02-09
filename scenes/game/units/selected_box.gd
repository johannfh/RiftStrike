extends Panel

# NOTE: For small visuals, scripting may be done in GDScript
func _on_selection_component_selection_changed(selected: bool) -> void:
	visible = selected
