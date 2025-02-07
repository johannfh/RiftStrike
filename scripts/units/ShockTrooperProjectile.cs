using Godot;

namespace Riftstrike.scripts.units {
    [GlobalClass]
    public partial class ShockTrooperProjectile : Area2D {
        public double Speed;

        public Vector2 Start { get => GlobalPosition; set => GlobalPosition = value; }

        public Vector2 Direction;

        public override void _PhysicsProcess(double delta) {
            var velocity = Direction * (float)delta * (float)Speed;
            GlobalPosition += velocity;
        }

        public override void _Ready() {
            var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            animatedSprite.Play();
            animatedSprite.AnimationFinished += () => animatedSprite.Play();
        }
    }
}