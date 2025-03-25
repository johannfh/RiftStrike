using Godot;

namespace Riftstrike.src.WaveShop
{
    public partial class WaveShop : Node2D
    {
        private const string SCENE_PATH = "res://src/WaveShop/wave_shop.tscn";


        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);

        public override void _Ready()
        {
            base._Ready();
            CursorSettings.LoadCursors();
        }
    }
}
