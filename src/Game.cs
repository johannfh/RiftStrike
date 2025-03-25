using System;
using System.Collections.Generic;
using System.Linq;
using Riftstrike.enemies;
using Riftstrike.src;
using Riftstrike.src.WaveShop;

namespace Riftstrike
{
    public partial class Game : Node2D
    {
        private const string SCENE_PATH = "res://src/game.tscn";

        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);

        const float WAVE_DURATION_SCALE = 5;
        const float MAX_WAVE_DURATION = 60;

        [Export]
        private Panel PauseBlurPanel;

        [Export]
        private PauseMenu PauseMenu;

        [Export]
        private AnimationPlayer UIAnimationPlayer;

        [Export]
        private Timer WaveEndTimer;

        [Export]
        private Timer SpawnEnemiesTimer;

        [Export]
        private int EnemySpawnCountScale = 3;

        [Export]
        private TileMapLayer Map;

        private bool gameOver;

        private double riftShardsAtStart;

        [Export]
        public Counter counter;

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
            var start = DateTime.Now;

            // get spawn positions
            var enemySpawnCount = 2 + EnemySpawnCountScale * (GlobalState.Wave - GlobalState.DEFAULT_WAVE);
            var positions = GetSafeEnemySpawnPoints(enemySpawnCount);

            var end = DateTime.Now;
            Debug.Print($"Time taken for spawn: {(end - start).TotalMilliseconds}ms");

            foreach (var pos in positions)
            {
                var enemy = Festerkin.New();
                enemy.GlobalPosition = pos;
                Map.AddChild(enemy);
            }
        }

        private static List<Vector2> GetSafeEnemySpawnPoints(int count)
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
            CursorSettings.LoadCursors();

            PauseMenu.PausedChanged += paused =>
            {
                if (paused == true)
                {
                    UIAnimationPlayer.Play("show_pause_menu");
                    GetTree().Paused = true;
                }
                // continue after animation has finished
                else UIAnimationPlayer.Play("hide_pause_menu");
            };

            UIAnimationPlayer.AnimationFinished += name =>
            {
                if (name == "hide_pause_menu")
                {
                    GetTree().Paused = false;

                }
            };

            SpawnEnemiesTimer.Timeout += SpawnEnemies;
            WaveEndTimer.WaitTime = Math.Min(10 + WAVE_DURATION_SCALE * GlobalState.Wave, MAX_WAVE_DURATION);
            WaveEndTimer.Timeout += EndWave;
            WaveEndTimer.Start();
            riftShardsAtStart = GlobalState.RiftShards;
            GD.Print($"Starting wave {GlobalState.Wave}!");
            SpawnUnits();
        }

        private void SpawnUnits()
            => GlobalState.UnitData
                // instantiate units from data
                .Select(d => d.InstantiateUnit())
                // spawn units
                .ForEach(u => Map.AddChild(u));

        private void EndWave()
        {
            GD.Print("Wave ended!");

            var riftShardsDiff = GlobalState.RiftShards - riftShardsAtStart;
            GD.Print($"Rift Shards: {GlobalState.RiftShards} (+{riftShardsDiff})");

            GD.Print("Opening wave shop!");
            GetTree().ChangeSceneToPacked(WaveShop.Scene);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (UnitManager.Instance.units.Count == 0 && !gameOver)
            {
                gameOver = true;
                HandleGameOver();
            }
        }

        private void HandleGameOver()
        {
            GD.Print("Game Over!");
            GetTree().ChangeSceneToPacked(TitleScreenUI.Scene);
        }
    }
}
