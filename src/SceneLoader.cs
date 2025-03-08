#nullable enable
using Godot;

namespace Riftstrike.src
{
    /// <summary>
    /// A static class that caches every scene after being accessed.
    /// </summary>
    public static class SceneLoader
    {
        private static PackedScene? titleScreenScene;
        public static PackedScene TitleScreenScene
            => titleScreenScene ??= GD.Load<PackedScene>("res://src/title_screen_ui.tscn");

        private static PackedScene? gameScene;
        public static PackedScene GameScene
            => gameScene ??= GD.Load<PackedScene>("res://src/game.tscn");

        private static PackedScene? waveShopScene;
        public static PackedScene WaveShopScene
            => waveShopScene ??= GD.Load<PackedScene>("res://src/WaveShop/wave_shop.tscn");

        private static PackedScene? gameSetupScene;
        public static PackedScene GameSetupScene
            => gameSetupScene ??= GD.Load<PackedScene>("res://src/GameSetup/game_setup.tscn");
    }
}
