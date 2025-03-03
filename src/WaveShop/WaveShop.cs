using Godot;

namespace Riftstrike.src.WaveShop
{
    public partial class WaveShop : Node2D
    {
        public override void _Ready()
        {
            base._Ready();
            CursorSettings.LoadCursors();
        }
    }
}
