

using System;

namespace Riftstrike.Src.Globals
{
    public partial class GlobalAudioStreamPlayer : Node
    {
        [Export]
        private AudioStreamPlayer UIElementHoveredPlayer;


        [Export]
        private AudioStreamPlayer UIElementPressedPlayer;

        private static GlobalAudioStreamPlayer Instance;

        public static void PlayUIElementHoveredSound()
            => Instance.UIElementHoveredPlayer.Play();

        public static void PlayUIElementPressedSound()
            => Instance.UIElementPressedPlayer.Play();


        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            if (Instance == this) Instance = null;
        }
    }
}