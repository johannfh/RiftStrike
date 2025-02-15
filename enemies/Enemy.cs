using Godot;

namespace Riftstrike.enemies
{
    [GlobalClass]
    public abstract partial class Enemy : Node2D
    {
        [Export(PropertyHint.None, "suffix:")]
        public double Experience = 10;

        public override void _Ready()
        {
            base._Ready();
            EnemyManager.Instance.enemies.Add(this);
        }

        public new void QueueFree()
        {
            var experience = GD.Load<PackedScene>("res://experience.tscn")
                .Instantiate<Experience>();
            experience.Value = Experience;
            experience.GlobalPosition = GlobalPosition;
            AddSibling(experience);
            base.QueueFree();
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            EnemyManager.Instance.enemies.Remove(this);
        }
    }
}