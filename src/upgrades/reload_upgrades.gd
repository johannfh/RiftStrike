@tool
class_name ReloadUpgrades extends EditorScript

const levelup_upgrades_path: String = "res://src/resources/levelup_upgrades"

func _run() -> void:
    print("Reloading upgrades in unit factory")
    var upgrade_factory: UpgradeFactory = get_scene() as UpgradeFactory
    print(upgrade_factory.LevelupUpgrades)
    upgrade_factory.LevelupUpgrades.clear()
    
    var levelup_upgrades_paths: PackedStringArray \
        = DirAccess.get_files_at(levelup_upgrades_path)
    
    print(levelup_upgrades_paths)
        
    var levelup_upgrades: Array[Upgrade] = []
    
    for path in levelup_upgrades_paths:
        var full_path = "%s/%s" % [levelup_upgrades_path, path]
        print(full_path)
        var upgrade = load(full_path) as Upgrade
        levelup_upgrades.append(upgrade)
    
    upgrade_factory.LevelupUpgrades.append_array(levelup_upgrades)
    upgrade_factory.notify_property_list_changed()
