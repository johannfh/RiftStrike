using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Godot;
using Riftstrike.components;

namespace Riftstrike.upgrades
{
    public interface IUpgrade
    {
        void Apply(StatsComponent target);
        Texture2D GetIcon();
    }

    public static class Upgrades
    {
        public static IUpgrade RandomUpgrade()
        {
            var rng = new RandomNumberGenerator();
            rng.Randomize();
            return RandomUpgrade(rng);
        }

        public static IUpgrade RandomUpgrade(RandomNumberGenerator rng)
        {
            return new List<IUpgrade> {
                new HealthModule(),
                new VitalityModule(),
                new DamageModule(),
                new RiftEnergyModule(),
            }.RandomElement(rng);
        }
    }
}