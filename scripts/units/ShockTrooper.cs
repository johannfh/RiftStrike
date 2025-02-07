using Godot;
using Riftstrike.scripts.components;

namespace Riftstrike.scripts.units {
	[GlobalClass]
	public partial class ShockTrooper : Unit, ICommandable, ISelectable {
		public CommandComponent GetCommandComponent()
			=> GetNode<CommandComponent>("CommandComponent");

		public SelectionComponent GetSelectionComponent()
			=> GetNode<SelectionComponent>("SelectionComponent");

		private Panel selectionBox;
		[Export] private double bulletSpeed = 300;

		private Vector2 GetProjectileStart()
			=> GetNode<Marker2D>("ProjectileSpawn").GlobalPosition;
		
		private Timer attackTimer;

		public override void _Ready() {
			GD.Print("ShockTrooper reporting!");
			selectionBox = GetNode<Panel>("SelectionBox");
			selectionBox.Visible = false;
			GetSelectionComponent().SelectionChanged += value => selectionBox.Visible = value;
			attackTimer = GetNode<Timer>("AttackTimer");
			attackTimer.Start();
		}

		public override void _PhysicsProcess(double delta) {
			if (attackTimer.IsStopped()) {
				ShootTowards(GetProjectileStart() + Vector2.Right);
				attackTimer.Start();
			}
		}

		private void ShootTowards(Vector2 target) {
			var bulletScene = GD.Load<PackedScene>("res://scenes/units/shock_trooper_projectile.tscn");
			var bullet = bulletScene.Instantiate() as ShockTrooperProjectile;
			var projectileStart = GetProjectileStart();
			bullet.Direction = projectileStart.DirectionTo(target);
			bullet.Start = projectileStart;
			bullet.Speed = bulletSpeed;
			GetTree().Root.AddChild(bullet);
		}
	}
}
