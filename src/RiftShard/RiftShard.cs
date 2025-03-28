namespace Riftstrike.src.RiftShard
{
    public partial class RiftShard : Area2D
    {
        private const string SCENE_PATH = "res://src/RiftShard/rift_shard.tscn";

        public static PackedScene Scene
            => GD.Load<PackedScene>(SCENE_PATH);

        public static RiftShard New()
            => Scene.Instantiate<RiftShard>();

        [ExportGroup("Textures")]

        [Export]
        private Texture2D TextureSmall;

        [Export]
        private Texture2D TextureMedium;

        [Export]
        private Texture2D TextureLarge;

        [Export(PropertyHint.Range, "0,50,1,or_greater")]
        private ulong ThresholdMedium { get; set; }

        [Export(PropertyHint.Range, "0,50,1,or_greater")]
        private ulong ThresholdLarge;

        [ExportGroup("Reward")]
        private ulong reward;
        [Export(PropertyHint.Range, "0,50,1,or_greater")]
        public ulong Reward
        {
            get => reward;
            set
            {
                reward = value;
                if (Sprite != null)
                {
                    var texture = GetTextureForRewardThreshold();
                    Sprite.Texture = texture;
                }
            }
        }

        [ExportGroup("Internal")]

        [Export]
        private Sprite2D Sprite;

        [Export]
        private AudioStreamPlayer2D CollectedSoundAudioStreamPlayer;

        public bool Collected { get; private set; } = false;
        public void Collect()
        {
            if (Collected) return;
            Collected = true;
            GlobalState.RiftShards += Reward;
            CollectedSoundAudioStreamPlayer.Play();
            Debug.Print($"{Name} was collected!");
        }

        private Texture2D GetTextureForRewardThreshold()
        {
            if (Reward < ThresholdMedium)
            {
                return TextureSmall;
            }
            if (Reward < ThresholdLarge)
            {
                return TextureMedium;
            }
            return TextureLarge;
        }

        public override void _Ready()
        {
            base._Ready();
            var texture = GetTextureForRewardThreshold();
            Sprite.Texture = texture;
        }
    }
}
