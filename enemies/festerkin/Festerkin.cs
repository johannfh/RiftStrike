using System.Linq;
using Godot;

namespace Riftstrike.enemies {
    public partial class Festerkin : Node2D {
        [Export] private double speed = 200;

        private NavigationAgent2D NavAgent
            => GetNode<NavigationAgent2D>("NavigationAgent2D");

        private AnimationPlayer AnimationPlayer
            => GetNode<AnimationPlayer>("AnimationPlayer");

        private Sprite2D Sprite
            => GetNode<Sprite2D>("Sprite2D");
        
        private Timer RecalculateTargetTimer
            => GetNode<Timer>("RecalculateTargetTimer");

        public override void _Ready() {
            base._Ready();
            AnimationPlayer.Play("walk");
            lastFramePos = GlobalPosition;
            RecalculateTargetTimer.Timeout += RecalculateTarget;
        }

        private void RecalculateTarget() {
            // TODO: implement calculation (first errors when none found; make it based on area and detection or closest player unit global?)
            NavAgent.TargetPosition = UnitSelectionManager.Instance.units.First().GlobalPosition;
        }

        public override void _Process(double delta) {
            base._Process(delta);
            UpdateSprite();
        }

        private Vector2 lastFramePos;
        private void UpdateSprite() {
            var dir = lastFramePos.DirectionTo(GlobalPosition);
            lastFramePos = GlobalPosition;
            var flipH = dir.X < 0;
            if (Sprite.FlipH != flipH) {
                Sprite.FlipH = flipH;
                AnimationPlayer.Advance(0);
            }
        }

        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);
            var nextPos = NavAgent.GetNextPathPosition();
            var dir = GlobalPosition.DirectionTo(nextPos);
            GlobalPosition += dir * (float)speed * (float)delta;
        }
    }
}
