#nullable enable

using Godot;
using Riftstrike.scripts.commands;

namespace Riftstrike.scripts.ui {
    public static class KeyMap {
        public static CommandShortcut? GetShortcut(CommandType commandType) {
            var path = $"res://shortcuts/{commandType}.tres";
            GD.Print($"Getting shortcut for {commandType}: {path}");
            if (ResourceLoader.Exists(path)) {
                var shortcut = GD.Load<CommandShortcut>(path);
                GD.Print($"Shortcut for {commandType}: {shortcut}");
                return shortcut;
            } else {
                GD.PrintErr($"Shortcut does not exists: {path}");
                return null;
            }
        } 
    }
}