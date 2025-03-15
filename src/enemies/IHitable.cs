using Riftstrike.src.units;

namespace Riftstrike.enemies
{
    public interface IHitable
    {
        void Hit(double damage, UnitData attacker);
    }
}
