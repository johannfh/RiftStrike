class_name State extends Node

## Emitted when the state finishes and wants to transition to another state.
signal transitioned(state: State, new_state_name: String)

func _ready() -> void:
	connect("transitioned", _on_transitioned)

func _on_transitioned(state: State, new_state_name):
	print("transitioning from state %s to %s" % 
		[state.name.to_lower(), new_state_name])

func enter() -> void:
	pass

func exit() -> void:
	pass

func update(_delta: float) -> void:
	pass

func physics_update(_delta: float) -> void:
	pass
