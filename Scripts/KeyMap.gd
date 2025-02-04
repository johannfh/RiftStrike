class_name KeyMap extends Resource

@export var type: Type = Type.Notification
@export var input_action: String
@export var icon: Texture2D
var handler: Callable

enum Type {
	# (append: bool)
	Notification,
	# (append: bool, target: Vector2)
	Positional,
	# (append: bool, target: Unit)
	Targeted,
}

func get_name_of_type(t: Type) -> String:
	if t == Type.Notification:
		return "Notification"
	if t == Type.Positional:
		return "Positional"
	if t == Type.Targeted:
		return "Targeted"
	return "TYPE_UNKNOWN"
