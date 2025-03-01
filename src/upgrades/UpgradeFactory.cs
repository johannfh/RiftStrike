using Godot;
using Godot.Collections;

namespace Riftstrike.upgrades
{
    [GlobalClass]
    public partial class UpgradeFactory : Node
    {
        public static UpgradeFactory Instance { get; private set; }

        [Export]
        private Array<Upgrade> LevelupUpgrades = new();

        private static readonly RandomNumberGenerator rng = new();

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
            rng.Randomize();
        }

        public static Upgrade RandomLevelupUpgrade()
            => RandomLevelupUpgrade(rng);


        public static Upgrade RandomLevelupUpgrade(RandomNumberGenerator rng)
            => Instance.LevelupUpgrades.RandomElement(rng);
    }
}
