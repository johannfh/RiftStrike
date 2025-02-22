using System;
using Godot;

namespace Riftstrike.components
{
    [GlobalClass]
    public partial class HealthComponent : Node
    {

        /// <summary>
        /// Emitted when Health reaches 0.
        /// </summary>
        [Signal] public delegate void DeathEventHandler();

        private double health = 1;

        /// <summary>
        /// Controls health points for this component.
        /// This should be managed from the owner (Unit/Enemy) Node after initialization.
        /// </summary>
        public double Health
        {
            get => health;
            set
            {
                health = value;
                if (health <= 0)
                {
                    health = 0;
                    EmitSignal(SignalName.Death);
                }
                if (health > MaxHealth)
                {
                    health = MaxHealth;
                }
            }
        }

        private double maxHealth;

        [Export]
        public double MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = value;
                Health = value;
            }
        }

        public override void _Ready()
        {
            base._Ready();
            Health = MaxHealth;
            GD.Print($"{Health}/{MaxHealth}hp");
        }

        public void Damage(double damage)
        {
            Health -= damage;
            GD.Print($"{GetParent().Name} took {damage} damage!");
        }

        public void Heal(double health)
        {
            Damage(-health);
        }
    }
}