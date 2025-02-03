extends Node

const SNIPER = preload("res://Units/Sniper/sniper.tscn")

func create_sniper() -> Sniper:
	return SNIPER.instantiate() as Sniper
