extends Node

const SNIPER = preload("res://Units/Sniper/sniper.tscn")
const TANK = preload("res://Units/Tank/tank.tscn")

func create_sniper() -> Sniper:
	return SNIPER.instantiate() as Sniper

func create_tank() -> Tank:
	return TANK.instantiate() as Tank
