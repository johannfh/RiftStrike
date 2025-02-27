using System.Collections.Generic;
using Godot;
using Riftstrike.src;
using Riftstrike.src.units;

namespace Riftstrike
{
    public partial class TitleScreenUI : Control
    {
        [Export]
        private Button PlayButton;

        public override void _Ready()
        {
            base._Ready();
            CursorSettings.Instance.Cursor = Cursor.Default;

            PlayButton.Pressed += () =>
            {
                GlobalState.Wave = 1;
                GlobalState.RiftShards = 0;
                GlobalState.UnitData.Clear();
                GlobalState.UnitData.AddRange(new List<UnitData>(){
                    new() { Type = UnitType.ShockTrooper },
                    new() { Type = UnitType.ShockTrooper },
                    // new() { Type = UnitType.RiftAssassin },
                });
                GetTree().ChangeSceneToPacked(SceneLoader.GameScene);
            };

        }
    }
}
