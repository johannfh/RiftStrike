using System;
using System.Collections.Generic;
using System.Linq;
using Riftstrike.components;
using Riftstrike.enemies;

namespace Riftstrike.src.units
{
    public partial class RiftAssassin : Unit, IWalk
    {
        #region Movement
        [ExportGroup("Movement")]

        [Export]
        private double speed;

        [Export]
        private float pushSpeed;

        private NavigationAgent2D NavAgent;

        #region Attacks
        [Export]
        private float baseAttackRange = 500;
        #endregion

        private readonly List<Vector2> targets = [];
        #endregion

        #region Selection
        private SelectableComponent SelectableComponent;

        private Panel SelectedPanel;

        private Panel HoveringPanel;
        #endregion

        #region Health Points
        private HitboxComponent HitboxComponent;
        private HealthComponent HealthComponent;
        #endregion

        [Export]
        private Timer AttackTimer;

        [Export]
        private PushComponent PushComponent;

        [Export]
        private double projectileBaseDamage = 10;

        private void AssignNodeReferences()
        {
            #region Movement
            NavAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
            #endregion

            #region Selection
            SelectableComponent = GetNode<SelectableComponent>("SelectableComponent");
            HoveringPanel = GetNode<Panel>("HoveringPanel");
            SelectedPanel = GetNode<Panel>("SelectedPanel");
            #endregion

            #region Health Points
            HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
            HealthComponent = GetNode<HealthComponent>("HealthComponent");
            #endregion
        }

        public override void _Ready()
        {
            base._Ready();
            AssignNodeReferences();
            HitboxComponent.Hit += HandleHit;
            HealthComponent.Death += HandleDeath;
            StatsRecalculated += HandleStatsRecalculated;
            UpdateStats();
        }

        private void HandleHit(double damage, Variant attacker)
        {
            // NOTE: Damage absorbtion goes here
            HealthComponent.Damage(damage);
        }

        private void HandleDeath()
        {
            // immortal when wave is over
            if (Game.WaveOver) return;

            GD.Print($"{Name} died!");
            QueueFree();
        }

        private void HandleStatsRecalculated()
        {
            HealthComponent.MaxHealth = CurrentStats.Health;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            #region Selection
            SelectedPanel.Visible = SelectableComponent.IsSelected;
            HoveringPanel.Visible = !SelectableComponent.IsSelected && SelectableComponent.IsHovered;
            #endregion
        }

        public override void _PhysicsProcess(double delta)
        {
            // freeze when game is over
            if (Game.WaveOver) return;

            base._PhysicsProcess(delta);
            #region Movement
            if (targets.Count != 0 && NavAgent.GetNextPathPosition() != targets.First())
            {
                NavAgent.TargetPosition = targets.First();
            }
            if (targets.Count != 0 && NavAgent.GetFinalPosition().DistanceTo(GlobalPosition) < 5)
            {
                targets.RemoveAt(0);
            }
            var nextPos = NavAgent.GetNextPathPosition();

            if (targets.Count != 0)
            {
                GlobalPosition = GlobalPosition.MoveToward(nextPos, (float)(speed * delta));
            }
            GlobalPosition += PushComponent.PushDirection * pushSpeed * (float)delta;
            #endregion

            if (AttackTimer.IsStopped()) Attack();
        }

        #region Attacks
        public void Attack()
        {
            // account for possibility of negative ProjectileBounces stat
            var count = 1 + Math.Max(CurrentStats.ProjectileBounces, 0);
            var enemies = GlobalPosition.GetNearbyEnemyChain(count, baseAttackRange);

            if (enemies.Count == 0) return;
            Debug.Print($"Got {enemies.Count}/{count} targets to shoot at");
            AttackTimer.Start();
            ShootBullet(enemies);
        }

        public void ShootBullet(IEnumerable<Enemy> enemies)
        {
            // TODO: Spawn bullet that flies by each position
            var bullet = RiftAssassinProjectile.New();
            bullet.UnitData = Data;

            bullet.Enemies = enemies.ToList();

            // NOTE: apply damage modifiers here
            // NOTE: limit to 0 (e.g. if negative damage modifiers from items)
            // to prevent healing enemies
            bullet.Damage = Math.Max(projectileBaseDamage * (CurrentStats.Damage / 100), 0);

            bullet.GlobalPosition = GlobalPosition;
            Debug.Print($"Enemies to shoot: [{string.Join(", ", enemies.Select(e => e.Name))}]");

            AddSibling(bullet);
        }
        #endregion

        #region Movement
        public void WalkTo(Vector2 targetPosition, bool append)
        {
            if (!append) targets.Clear();
            targets.Add(targetPosition);
        }
        #endregion
    }
}
