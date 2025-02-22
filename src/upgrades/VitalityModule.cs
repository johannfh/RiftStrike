using Godot;
using Riftstrike.components;

namespace Riftstrike.upgrades
{
    /// <summary>
    /// Increases regeneration by 3. 
    /// </summary>
    public partial class VitalityModule : IUpgrade
    {
        public void Apply(StatsComponent target)
        {
            target.Regeneration += 3;
        }

        public Texture2D GetIcon()
        {
            throw new System.NotImplementedException();
        }
    }
}