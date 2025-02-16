using System.Collections.Generic;
using Godot;

namespace Riftstrike.components
{
    [GlobalClass]
    public partial class LevelComponent : Area2D
    {
        public ulong Level;
        public double Experience;

        public override void _Ready()
        {
            base._Ready();
            AreaEntered += OnAreaEntered;
        }

        private readonly List<Experience> experiences = new();

        private void OnAreaEntered(Area2D area)
        {
            if (area is Experience exp && !exp.Collected)
            {
                experiences.Add(exp);
                exp.Collected = true;
                Experience += exp.Value;
            }
        }

        private static readonly double ExperienceVelocity = 500;
        private static readonly double CollectThreshhold = 10;
        public override void _PhysicsProcess(double delta)
        {
            // batch removal
            var experienceToRemove = new List<Experience>();

            foreach (var experience in experiences)
            {
                experience.GlobalPosition = experience.GlobalPosition.MoveToward(
                    GlobalPosition, (float)(ExperienceVelocity * delta)
                );

                if (experience.GlobalPosition.DistanceTo(GlobalPosition) < CollectThreshhold)
                {
                    experience.QueueFree();
                    experienceToRemove.Add(experience);
                }
            }
            experienceToRemove.ForEach(exp => experiences.Remove(exp));
        }
    }
}
