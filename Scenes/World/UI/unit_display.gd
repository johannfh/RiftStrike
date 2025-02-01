class_name UnitDisplay
extends Control

var unit: Unit

const MODULATE_SELECTED = Color("#ffffffff")
const MODULATE_ALIVE = Color("#ffffffaa")
const MODULATE_DEAD = Color("#ff444488")

@onready var texture_rect: TextureRect = $TextureRect

func _ready() -> void:
	texture_rect.texture = unit.icon

func get_modulate_for_unit(u: Unit) -> Color:
	if u.hp > 0:
		if u.selected:
			return MODULATE_SELECTED
		else:
			return MODULATE_ALIVE
	else:
		return MODULATE_DEAD

func _process(delta: float) -> void:
	texture_rect.modulate = get_modulate_for_unit(unit)
