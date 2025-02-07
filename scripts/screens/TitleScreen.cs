using Godot;
using Riftstrike.scripts.utils;

namespace Riftstrike.scripts.screens {
    public partial class TitleScreen : VBoxContainer {
        private Button startButton;

        public override void _Ready() {
            startButton = GetNode<Button>("StartButton");
            startButton.Pressed += () => {
                ScreenLoader.SetScreen("world.tscn");
            };
        }
    }
}
