extends Node2D

const GAME_OVER = preload("res://Scenes/GameOver/GameOver.tscn")

@onready var selection_box: SelectionBox = $SelectionBox
@onready var unit_manager: UnitManager = $UnitManager
@onready var unit_control: UnitControl = $UnitControl

var hovering_selectables: Array[SelectableComponent] = []

func _ready() -> void:
	for i in range(3):
		var sniper := UnitFactory.create_sniper()
		unit_manager.add_unit(sniper)

func _on_move_command(target: Vector2, append: bool) -> void:
	return
	print("move units to %v" % target)
	var selected: Array[SelectableComponent]
	var cmd = GlobalMovementCommand.new(target)
	selected.assign(Utils.get_selectables().filter(Utils.filter_selected))
	for s in selected:
		if append:
			s.commands.append(cmd)
		else:
			s.commands = [cmd]
		print("commands %s" % str(s.commands))

func _process(_delta: float) -> void:
	pass

func _physics_process(_delta: float) -> void:
	for s in Utils.get_selectables():
		# deselect hovering
		if s.state == Utils.SelectionState.Hovering:
			s.state = Utils.SelectionState.NotSelected

	var selectables: Array[SelectableComponent]
	selectables.assign(selection_box.get_overlapping_areas())
	hovering_selectables.assign(selectables)
	
	if Input.is_action_just_pressed("escape"):
		var selected := Utils.get_selectables() \
			.filter(Utils.filter_selected)
		for s in selected:
			s.state = Utils.SelectionState.NotSelected
	elif Input.is_action_just_released("left_click"):
		var selected := Utils.get_selectables() \
			.filter(Utils.filter_selected)
		
		if not Input.is_action_pressed("shift") and len(selectables) > 0:
			# deselect old selected selectables
			for s in selected:
				s.state = Utils.SelectionState.NotSelected
		
		# select new selectables
		for s in selectables:
			s.state = Utils.SelectionState.Selected
	else:
		for s in selectables:
			if s.state != Utils.SelectionState.Selected:
				s.state = Utils.SelectionState.Hovering
