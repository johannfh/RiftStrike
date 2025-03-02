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

        [Export]
        public string Label = "";

        [Export]
        public Rarity Rarity = Rarity.Common;

        /// <summary>
        /// Applies the upgrade to the specified target's stats.
        /// </summary>
        /// <param name="target">The stats component to which the upgrade will be applied.</param>
        public abstract void Apply(Stats target);

    }

    public enum Rarity
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5,
    }
}