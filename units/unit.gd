class_name Unit
extends CharacterBody2D

var commands: Array[Command] = []
@export var upgrades: Array[UnitUpgrade] = []

func add_upgrade(upgrade: UnitUpgrade) -> void:
	self.upgrades.append(upgrade)

func remove_upgrade(upgrade: UnitUpgrade) -> void:
	var idx = upgrades.find(upgrade)
	if idx != -1:
		upgrades.remove_at(idx)

func takes_upgrade(upgrade: UnitUpgrade) -> bool:
	return false
