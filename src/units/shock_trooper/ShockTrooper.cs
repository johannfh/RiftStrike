using Godot;
using Godot.Collections;
using Riftstrike.components;
using Riftstrike.upgrades;
using System.Collections.Generic;
using System.Linq;

namespace Riftstrike.src.units
{
    [GlobalClass]
    public partial class ShockTrooper : Unit, IWalk
    {
        [ExportGroup("Movement")]
        [Export] private float speed = 200;
        [Export] private float pushSpeed = 50;
        private readonly List<Vector2> targets = new();

        [ExportGroup("Attacks")]
        [Export] private float projectileSpeed = 300;
        [Export] private float projectileBaseDamage = 10;

        private NavigationAgent2D NavAgent
            => GetNode<NavigationAgent2D>("NavigationAgent2D");

        private PushComponent PushComponent
            => GetNode<PushComponent>("PushComponent");

        private SelectableComponent SelectableComponent
            => GetNode<SelectableComponent>("SelectableComponent");

        private HitboxComponent HitboxComponent
            => GetNode<HitboxComponent>("HitboxComponent");

        private HealthComponent HealthComponent
            => GetNode<HealthComponent>("HealthComponent");

        private Timer RegenerationTimer
            => GetNode<Timer>("RegenerationTimer");

        private Sprite2D Sprite
            => GetNode<Sprite2D>("Sprite2D");

        private Panel selectedPanel;
        private Panel hoveringPanel;
        private Timer attackTimer;
        private LevelComponent levelComponent;

        private bool AttackReady = false;

        private void AssignNodeReferences()
        {
            selectedPanel = GetNode<Panel>("SelectedPanel");
            hoveringPanel = GetNode<Panel>("HoveringPanel");
            attackTimer = GetNode<Timer>("AttackTimer");
            levelComponent = GetNode<LevelComponent>("LevelComponent");
        }

        public override void _Ready()
        {
            base._Ready();
            AssignNodeReferences();
            StatsRecalculated += HandleStatsRecalculated;
            UpdateStats();
            RegenerationTimer.Timeout += HandleRegen;
            HitboxComponent.Hit += HandleHit;
            HealthComponent.Death += HandleDeath;
            attackTimer.Timeout += () => AttackReady = true;
            levelComponent.Levelup += HandleLevelup;
        }

        private void HandleLevelup(ulong level)
        {
            var upgrade = UpgradesFactory.RandomLevelupUpgrade();
            GD.Print($"Got random upgrade: {upgrade.ResourcePath}");
            Upgrades.Add(upgrade);
            UpdateStats();
        }


        private void HandleDeath()
        {
            GD.Print($"{Name} died!");
            QueueFree();
        }

        private void HandleRegen()
        {
            HealthComponent.Health = Mathf.Min(
                HealthComponent.Health + TargetStats.Regeneration * RegenerationTimer.WaitTime,
                TargetStats.Health
            );
        }

        private void HandleHit(double damage)
        {
            // NOTE: Damage absorbtion goes here
            HealthComponent.Damage(damage);
        }

        private void HandleStatsRecalculated()
        {
            HealthComponent.MaxHealth = TargetStats.Health;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            selectedPanel.Visible = SelectableComponent.IsSelected;
            hoveringPanel.Visible = !SelectableComponent.IsSelected && SelectableComponent.IsHovered;
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (targets.Any() && NavAgent.TargetPosition != targets.First())
            {
                NavAgent.TargetPosition = targets.First();
            }
            if (targets.Any() && NavAgent.GetFinalPosition().DistanceTo(GlobalPosition) < 5)
            {
                targets.RemoveAt(0);
            }
            var nextPos = NavAgent.GetNextPathPosition();

            if (targets.Any())
            {
                GlobalPosition = GlobalPosition.MoveToward(nextPos, speed * (float)delta);
            }
            GlobalPosition += PushComponent.PushDirection * pushSpeed * (float)delta;

            if (AttackReady)
            {
                var enemies = EnemyManager.Instance.enemies;
                if (enemies.Any())
                {
                    var closestTarget = enemies
                        .OrderBy(e => e.GlobalPosition.DistanceTo(GlobalPosition))
                        .First();

                    if (IsInRange(closestTarget))
                    {
                        ShootTowards(closestTarget);
                        attackTimer.Start();
                        AttackReady = false;
                    }
                }
            }
        }

        private bool IsInRange(Node2D node2D)
        {
            return IsInRange(node2D.GlobalPosition);
        }

        private bool IsInRange(Vector2 position)
        {
            return GlobalPosition.DistanceTo(position) < TargetStats.Range;
        }

        private void ShootTowards(Node2D node2D)
        {
            ShootTowards(node2D.GlobalPosition);
        }

        private void ShootTowards(Vector2 target)
        {
            // calculate parameters
            var bulletPos = GlobalPosition;
            var bulletDir = GlobalPosition.DirectionTo(target);
            var bulletVelocity = bulletDir * projectileSpeed;
            var bulletRange = TargetStats.Range * 2;

            // NOTE: apply damage modifiers here
            var bulletDamage = projectileBaseDamage * (TargetStats.Damage / 100);


            // instantiate bullet
            var bullet = ShockTrooperProjectile.New();
            bullet.GlobalPosition = bulletPos;
            bullet.Velocity = bulletVelocity;
            bullet.Damage = bulletDamage;
            bullet.Range = bulletRange;

            // spawn in tree
            AddSibling(bullet);
        }

        public void WalkTo(Vector2 targetPosition, bool append)
        {
            if (!append) targets.Clear();
            targets.Add(targetPosition);
        }
    }
}
