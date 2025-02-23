using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.components;
using Riftstrike.enemies;

namespace Riftstrike
{
    public partial class Game : Node2D
    {
        [Export]
        private RandomTimer SpawnEnemiesTimer;

        [Export]
        private int enemySpawnCount = 3;

        [Export]
        private TileMapLayer Map;

        private bool gameOver;


        private static Vector2 MapCellToPosition(Vector2I Cell)
        {
            const int TILE_WIDTH = 64;
            const int TILE_HEIGHT = 64;

            var pos = new Vector2()
            {
                // Note: this picks the edge of a tile and not the center
                X = Cell.X * TILE_WIDTH,
                Y = Cell.Y * TILE_HEIGHT,
            };
            return pos;
        }

        private void SpawnEnemies()
        {
#if DEBUG
            var start = DateTime.Now;
#endif
            var positions = GetSafeEnemySpawnPoints(5);
#if DEBUG
            var end = DateTime.Now;
            GD.Print($"Time taken for spawn: {(end - start).TotalMilliseconds}ms");
#endif
            foreach (var pos in positions)
            {
                var enemy = GD.Load<PackedScene>("res://src/enemies/festerkin/festerkin.tscn")
                    .Instantiate<Enemy>();
                enemy.GlobalPosition = pos;
                Map.AddChild(enemy);
            }
        }

        private static IEnumerable<Vector2> GetSafeEnemySpawnPoints(int count)
        {
            var map = GetNavMap();
            var result = new List<Vector2>();
            for (int i = 0; i < count; i++)
            {
                result.Add(GetSafeEnemySpawnPoint(map));
            }
            return result;
        }

        private static Vector2 GetSafeEnemySpawnPoint(Rid map)
        {
            while (true)
            {
                // get a random navigable position
                var position = NavigationServer2D.MapGetRandomPoint(
                    map,
                    (uint)NavigationLayer.Main,
                    true
                );

                // check if position is safe
                var isSafe = true;
                foreach (var unit in UnitManager.Instance.units)
                {
                    if (unit.GlobalPosition.DistanceTo(position) <= unit.SafeDistance)
                    {
                        isSafe = false;
                        break;
                    }
                }

                // return first safe position that is found
                if (isSafe)
                {
                    return position;
                }
            }
        }

        private static Rid GetNavMap()
        {
            var maps = NavigationServer2D.GetMaps();
            Debug.Assert(maps.Count == 1, "There must be exactly one nav map.");
            return maps.First();
        }

        public override void _Ready()
        {
            base._Ready();

            SpawnEnemiesTimer.Timeout += SpawnEnemies;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (!UnitManager.Instance.units.Any() && !gameOver)
            {
                gameOver = true;
                HandleGameOver();
            }
        }

        private void HandleGameOver()
        {
            var titleScreenScene = GD.Load<PackedScene>("res://src/title_screen_ui.tscn");
            GetTree().ChangeSceneToPacked(titleScreenScene);
        }
    }
}
