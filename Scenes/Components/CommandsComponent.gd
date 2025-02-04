class_name CommandsComponent extends Node

signal command(cmd: Command, append: bool)

@export var supported: Array[Command.Type] = []
