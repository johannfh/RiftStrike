using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.components;

namespace Riftstrike.upgrades
{
    [GlobalClass]
    public abstract partial class Upgrade : Resource
    {
        [Export]
        public Texture2D Icon;

        /// <summary>
        /// Applies the upgrade to the specified target's stats.
        /// </summary>
        /// <param name="target">The stats component to which the upgrade will be applied.</param>
        public abstract void Apply(Stats target);
    }

    public static class UpgradesFactory
    {
        private static readonly Upgrade[] LevelupUpgrades;
        private static readonly RandomNumberGenerator randomNumberGenerator;

        static UpgradesFactory()
        {
            const string LEVELUP_UPGRADES_PATH = "res://src/resources/levelup_upgrades";
            var paths = DirAccess.GetFilesAt(LEVELUP_UPGRADES_PATH)
                .Select(f => $"{LEVELUP_UPGRADES_PATH}/{f}");

            GD.Print($"Upgrades: {string.Join(", ", paths)}");

            LevelupUpgrades = paths
                .Select(p => GD.Load<Upgrade>(p))
                .ToArray();

            randomNumberGenerator = new RandomNumberGenerator();
            randomNumberGenerator.Randomize();
        }

        public static Upgrade RandomLevelupUpgrade()
            => RandomLevelupUpgrade(randomNumberGenerator);


        public static Upgrade RandomLevelupUpgrade(RandomNumberGenerator rng)
            => LevelupUpgrades.RandomElement(rng);
    }
}