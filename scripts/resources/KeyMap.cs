using Godot;
using Godot.Collections;
using Riftstrike.scripts.commands;

namespace Riftstrike.scripts.resources {
    using Mapping = Dictionary<Shortcut, Variant>;

    [GlobalClass]
    public partial class KeyMap : Resource {
        [Export] public Mapping mapping = new();

        /// <summary>
        /// Runtime validation to check correctness of this KeyMap.
        /// </summary>
        public bool IsValidMapping() {
            foreach (var entry in mapping) {
                Variant key = entry.Key;
                Variant value = entry.Value;

                // All keys have to be shortcuts
                if (!(key is Variant k && k.As<Shortcut>() is Shortcut _)) {
                    GD.Print($"INVALID KEY {k}");
                    return false;
                }

                // All values have to be either a CommandBinding
                // or another valid KeyMap (allows for nesting)
                if (value is Variant v) {
                    if (v.As<CommandBinding>() is CommandBinding _) {
                        continue;
                    }

                    if (v.As<KeyMap>() is KeyMap subMap && subMap.IsValidMapping()) {
                        continue;
                    }
                    GD.Print($"INVALID VALUE {v}");
                }

                GD.Print("INVALID KEYMAP");

                // The value could not be matched correctly
                return false;
            }
            return true;
        }
    }

    [GlobalClass]
    public partial class CommandBinding : Resource {
        [Export] CommandType commandType;
    }
}