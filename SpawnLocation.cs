using System;
using Godot;

namespace Riftstrike
{
    [GlobalClass]
    public partial class SpawnLocation : Marker2D
    {
        [Export] private RandomTimer SpawnDelayTimer;
        [Export] private Panel Panel;

        private Action Callback;

        public ulong LastUsed { get; private set; }
        public ulong MsecSinceLastUsed => Time.GetTicksMsec() - LastUsed;

        public override void _Ready()
        {
            base._Ready();
            SpawnDelayTimer.Timeout += () =>
            {
                Callback?.Invoke();
                Callback = null;
                LastUsed = Time.GetTicksMsec();
            };
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            Panel.Visible = !SpawnDelayTimer.IsStopped();
        }

        public void Spawn(Action action)
        {
            Callback = action;
            SpawnDelayTimer.StartRandom();
            // make spawns maximally unlikely during delayed spawn
            LastUsed = ulong.MaxValue;
        }
    }
}
