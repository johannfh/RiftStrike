using Godot;
using Godot.Collections;
using Riftstrike.src.units;
using System.Linq;


namespace Riftstrike.src.GameSetup
{
    public partial class GameSetupUI : Control
    {
        [Export]
        private Array<UnitData> UnitData = new();

        [Export]
        private HBoxContainer UnitSelectionBoxesContainer;

        [Export]
        private Button FightButton;

        [Export]
        private int MaxUnits = 4;

        public override void _Ready()
        {
            base._Ready();
            // reset global state for next game
            GlobalState.Reset();

            // remove remaining children from unit selection boxes container (if any)
            UnitSelectionBoxesContainer.GetChildren()
                .ForEach(child => UnitSelectionBoxesContainer.RemoveChild(child));

            // render selection boxes for every unit data
            var unitSelectionBoxScene = GD.Load<PackedScene>("res://src/GameSetup/unit_selection_box.tscn");
            foreach (var ud in UnitData)
            {
                var unitSelectionBox = unitSelectionBoxScene.Instantiate<UnitSelectionBox>();
                unitSelectionBox.Icon = ud.Icon;
                unitSelectionBox.Selected += (selected) =>
                {
                    Debug.Print(selected);
                    if (selected)
                    {
                        if (GlobalState.UnitData.Count < MaxUnits)
                        {
                            GlobalState.UnitData.Add(ud);
                            unitSelectionBox.IsSelected = true;
                            Debug.Print($"Units chosen for next game: {string.Join(", ", GlobalState.UnitData.Select(ud => ud.ResourcePath.Split("/").Last()))}");
                        }
                    }
                    else
                    {
                        if (GlobalState.UnitData.Contains(ud))
                        {
                            GlobalState.UnitData.Remove(ud);
                            unitSelectionBox.IsSelected = false;
                            Debug.Print($"Units chosen for next game: {string.Join(", ", GlobalState.UnitData.Select(ud => ud.ResourcePath.Split("/").Last()))}");
                        }
                    }

                };
                UnitSelectionBoxesContainer.AddChild(unitSelectionBox);
            }

            FightButton.Pressed += () =>
            {
                if (!GlobalState.UnitData.Any()) return;
                GetTree().ChangeSceneToPacked(SceneLoader.GameScene);
            };
        }
    }
}
