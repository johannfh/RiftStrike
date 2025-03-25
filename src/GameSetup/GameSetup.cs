using Godot;
using System;

namespace Riftstrike.src.GameSetup
{
    public partial class GameSetup : Node2D
    {
        private const string SCENE_PATH = "res://src/GameSetup/game_setup.tscn";

        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);
    }
}
