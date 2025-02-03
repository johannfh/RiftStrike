class_name CommandsComponent extends Node

var commands: Array[Command] = []
@export var supported: Array[CommandTypes] = []

enum CommandTypes {
	Movement,
	Attack,
	Stop,
	Cloak,
	Siege,
	Unsiege,
}
