class_name Command extends Object

var type: Type

# WARNING: From v1.0 on the values should
# not change anymore for backwards compatability
enum Type {
	# Global Namespace (0-99)
	# Special
	Empty = 0,
	Stop = 1,
	
	# Movement
	Move = 10,
	#TODO: Follow = 11,
	
	# Attacks
	Attack = 20,
	#TODO: Chase = 21,
	
	# Assasin Namespace (100-199)
	Cloak = 100,
	Decloak = 101,
	
	# Tank Namespace (200-299)
	Siege = 200,
	Unsiege = 201,
}

const STOP = preload("res://Resources/KeyMaps/stop.tres")
const MOVE = preload("res://Resources/KeyMaps/move.tres")
const ATTACK = preload("res://Resources/KeyMaps/attack.tres")

const CLOAK = preload("res://Resources/KeyMaps/cloak.tres")
const DECLOAK = preload("res://Resources/KeyMaps/decloak.tres")

const SIEGE = preload("res://Resources/KeyMaps/siege.tres")
const UNSIEGE = preload("res://Resources/KeyMaps/unsiege.tres")

# WARNING: returns null for missing keymaps
static func get_keymap(cmd_type: Type) -> KeyMap:
	match cmd_type:
		Type.Stop: return STOP
		Type.Move: return MOVE
		Type.Attack: return ATTACK
		Type.Cloak: return CLOAK
		Type.Decloak: return DECLOAK
		Type.Siege: return SIEGE
		Type.Unsiege: return UNSIEGE
	
	# fail during debug
	assert(false, "unknown keymap type: %s" % cmd_type)
	return null
