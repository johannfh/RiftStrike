namespace Riftstrike.src
{
    public partial class GlobalStatsDisplay : VBoxContainer
    {
        public override void _Process(double delta)
        {
            base._Process(delta);
            GetNode<Label>("%RiftShardsLabel").Text = $"{GlobalState.RiftShards}";
        }
    }
}
