class_name CommandsComponent extends Node

@warning_ignore("unused_signal")
signal command(cmd: Command, append: bool)

@export var supported: Array[Command.Type] = []
