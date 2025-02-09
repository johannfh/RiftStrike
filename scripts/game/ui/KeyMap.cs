#nullable enable

using System.Runtime.InteropServices;
using Godot;

namespace Riftstrike.scripts.game.ui {
    public static class KeyMap {
        public static CommandShortcut? GetCommandShortcut(string commandName) {
            var path = $"res://shortcuts/{commandName}.tres";
            GD.Print($"Getting shortcut for {commandName}: {path}");
            if (ResourceLoader.Exists(path)) {
                var commandShortcut = GD.Load<CommandShortcut>(path);
                var text = commandShortcut.shortcut.GetAsText();
                var type = commandShortcut.shortcutType;
                var msg = $"Shortcut for \"{commandName}\" is \"{text}\" ({type})";
                GD.Print(msg);
                return commandShortcut;
            } else {
                GD.PrintErr($"Shortcut does not exists: {path}");
                return null;
            }
        }
    }
}