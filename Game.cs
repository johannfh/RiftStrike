using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.enemies;

namespace Riftstrike
{
    public partial class Game : Node2D
    {
        private readonly List<SpawnLocation> spawnLocations = new();

        [Export]
        private RandomTimer SpawnEnemiesTimer;

        [Export]
        private int unitSpawnCount = 3;

        [Export]
        private TileMapLayer Map;

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

        public override void _Ready()
        {
            base._Ready();

            var spawnLocationScene = GD.Load<PackedScene>("res://spawn_location.tscn");
            foreach (var cell in Map.GetUsedCells())
            {
                var pos = MapCellToPosition(cell);
                var spawnLocation = spawnLocationScene.Instantiate<SpawnLocation>();
                spawnLocation.GlobalPosition = pos;
                Map.AddChild(spawnLocation);
            }

            spawnLocations.AddRange(GetTree()
                .GetNodesInGroup("spawn_location")
                .OfType<SpawnLocation>());

            SpawnEnemiesTimer.Timeout += () =>
            {
                var locations = new List<SpawnLocation>();
                var perUnit = Math.Min(unitSpawnCount, spawnLocations.Count) / UnitSelectionManager.Instance.units.Count;

                foreach (var unit in UnitSelectionManager.Instance.units)
                {
                    locations.AddRange(GetRandomSpawnLocations(perUnit, unit.GlobalPosition));
                }

                GD.Print($"[{string.Join(", ", locations.Select(l => l.GlobalPosition))}]");
                locations.ForEach(l => l.Spawn(() =>
                {
                    GD.Print($"Spawning for {l.Name}");
                    var pos = l.GlobalPosition;
                    var enemy = GD.Load<PackedScene>("res://enemies/festerkin/festerkin.tscn")
                        .Instantiate() as Enemy;
                    enemy.GlobalPosition = pos;
                    Map.AddChild(enemy);
                }));
            };
        }

        public IEnumerable<SpawnLocation> GetRandomSpawnLocations(
            int count,
            Vector2 center
        )
        {
            Debug.Assert(spawnLocations.Count >= count, "Not enough spawn locations exist");

            // calculate weights
            var weights = new Dictionary<double, SpawnLocation>();
            foreach (var location in spawnLocations)
            {
                var distance = location.GlobalPosition.DistanceTo(center);
                var timeFactor = Mathf.Sqrt(location.MsecSinceLastUsed);
                var weight = 1 / (distance + 1) * timeFactor;
                weights[weight] = location;
            }

            // normalize weights
            var totalWeight = weights.Keys.Sum();
            var normalizedWeights = weights
                .ToDictionary(
                    kvp => kvp.Key / totalWeight,
                    kvp => kvp.Value
                );


            var random = new Random();
            var selectedLocations = new List<SpawnLocation>();
            for (int i = 0; i < count; i++)
            {
                var randomValue = random.NextDouble();
                var cumulativeWeight = 0.0;
                foreach (var kvp in normalizedWeights)
                {
                    cumulativeWeight += kvp.Key;
                    if (randomValue <= cumulativeWeight)
                    {
                        selectedLocations.Add(kvp.Value);
                        break;
                    }
                }
            }

            return selectedLocations;
        }
    }
}
