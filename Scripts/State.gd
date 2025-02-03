class_name State extends Node

## Emitted when the state finishes and wants to transition to another state.
@warning_ignore("unused_signal")
signal transitioned(state: State, new_state_name: String)

func enter() -> void:
	pass

func exit() -> void:
	pass

func update(_delta: float) -> void:
	pass

func physics_update(_delta: float) -> void:
	pass
