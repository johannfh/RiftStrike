using Godot;

namespace Riftstrike.src.units
{
    [GlobalClass]
    public partial class Stats : Resource
    {
        [Export(PropertyHint.None, "0,100,1,or_greater,suffix:hp")]
        public float Health = 50;

        [Export(PropertyHint.None, "0,100,1,or_greater,suffix:per second")]
        public float Regeneration = 0;

        [Export(PropertyHint.None, "0,100,1,or_greater,suffix:RE")]
        public float RiftEnergy = 0;

        [Export(PropertyHint.Range, "0,100,1,or_greater,suffix:%")]
        public float Damage = 100;


        [Export(PropertyHint.Range, "0,100,1,or_greater,suffix:pixels")]
        public float Range = 500;

        public void SetValuesTo(Stats stats)
        {
            Health = stats.Health;
            Regeneration = stats.Regeneration;
            RiftEnergy = stats.RiftEnergy;
            Damage = stats.Damage;
            Range = stats.Range;
        }
    }
}