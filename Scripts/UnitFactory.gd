extends Node

const SHOCK_TROOPER = preload("res://Units/ShockTrooper/shock_trooper.tscn")

func create_shock_trooper() -> ShockTrooper:
	return SHOCK_TROOPER.instantiate() as ShockTrooper
