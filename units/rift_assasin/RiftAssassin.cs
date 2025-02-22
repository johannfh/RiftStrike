using System.Collections.Generic;
using System.Linq;
using Godot;
using Riftstrike.components;

namespace Riftstrike.units
{
    public partial class RiftAssassin : Unit, IWalk
    {
        #region Movement
        [ExportGroup("Movement")]

        [Export]
        private double speed;

        [Export]
        private double pushSpeed;

        private NavigationAgent2D NavAgent;

        private PushComponent PushComponent;

        #region Attacks
        [Export]
        private float baseAttackRange = 500;
        #endregion

        private readonly List<Vector2> targets = new();
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

        #region Stats
        private StatsComponent BaseStatsComponent;
        private StatsComponent TargetStatsComponent;
        #endregion

        #region Upgrades
        private UpgradeComponent UpgradeComponent;
        #endregion


        private void AssignNodeReferences()
        {
            #region Movement
            NavAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
            PushComponent = GetNode<PushComponent>("PushComponent");
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

            #region Stats
            BaseStatsComponent = GetNode<StatsComponent>("BaseStatsComponent");
            TargetStatsComponent = GetNode<StatsComponent>("TargetStatsComponent");
            #endregion

            #region Upgrades
            UpgradeComponent = GetNode<UpgradeComponent>("UpgradeComponent");
            #endregion
        }

        public override void _Ready()
        {
            base._Ready();
            AssignNodeReferences();
            HitboxComponent.Hit += HandleHit;
            HealthComponent.Death += HandleDeath;
            UpgradeComponent.StatsRecalculated += HandleStatsRecalculated;
            UpgradeComponent.Update();
        }

        private void HandleHit(double damage)
        {
            // NOTE: Damage absorbtion goes here
            GD.Print("AHH");
            HealthComponent.Damage(damage);
        }

        private void HandleDeath()
        {
            GD.Print($"{Name} died!");
            QueueFree();
        }

        private void HandleStatsRecalculated()
        {
            // heal up to new max health on upgrade
            HealthComponent.MaxHealth = TargetStatsComponent.Health;
            HealthComponent.Health = TargetStatsComponent.Health;
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
            base._PhysicsProcess(delta);
            #region Movement
            if (targets.Any() && NavAgent.GetNextPathPosition() != targets.First())
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
                GlobalPosition = GlobalPosition.MoveToward(nextPos, (float)(speed * delta));
            }
            #endregion
        }

        #region Attacks
        public void Attack()
        {
            var enemies = GlobalPosition.GetNearbyEnemyChain(5, baseAttackRange);
            if (!enemies.Any()) return;
            var positions = enemies.Select(enemy => enemy.GlobalPosition).ToList();
            positions.Insert(0, GlobalPosition);
            ShootBullet(positions);
        }

        public void ShootBullet(IEnumerable<Vector2> positions)
        {
            Debug.Assert(positions.Count() >= 2, "There have to be at least 2 positions to shoot.");
            for (int i = 1; i < positions.Count(); i++)
            {
                var from = positions.ElementAt(i - 1);
                var to = positions.ElementAt(i);
            }
            // TODO: Spawn bullet that flies by each position
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
