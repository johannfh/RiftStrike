using Godot.Collections;
using Riftstrike.src.units;
using Riftstrike.Src.Globals;
using System.Linq;


namespace Riftstrike.src.GameSetup
{
    public partial class GameSetupUI : Control
    {
        [Export]
        private Array<UnitData> UnitData = [];

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
                .ForEach(UnitSelectionBoxesContainer.RemoveChild);

            // render selection boxes for every unit data

            foreach (var udBase in UnitData)
            {
                // clone the unit data to prevent modifying the original stats
                var udCopy = udBase.Duplicate<UnitData>();

                var unitSelectionBox = UnitSelectionBox.New();
                unitSelectionBox.Icon = udCopy.Icon;
                unitSelectionBox.Selected += (selected) =>
                {
                    Debug.Print(selected);
                    if (selected)
                    {
                        if (GlobalState.UnitData.Count < MaxUnits)
                        {
                            GlobalState.UnitData.Add(udCopy);
                            unitSelectionBox.IsSelected = true;
                            Debug.Print($"Units chosen for next game: {string.Join(", ", GlobalState.UnitData.Select(ud => ud.ResourcePath.Split("/").Last()))}");
                        }
                    }
                    else
                    {
                        GlobalState.UnitData.Remove(udCopy);
                        unitSelectionBox.IsSelected = false;
                        Debug.Print($"Units chosen for next game: {string.Join(", ", GlobalState.UnitData.Select(ud => ud.ResourcePath.Split("/").Last()))}");
                    }

                };
                UnitSelectionBoxesContainer.AddChild(unitSelectionBox);
            }

            FightButton.Pressed += () =>
            {
                GlobalAudioStreamPlayer.PlayUIElementPressedSound();
                if (GlobalState.UnitData.Count == 0) return;
                GetTree().ChangeSceneToPacked(Game.Scene);
            };
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ButtonScaleTweenHover(FightButton, 1.2F, 0.3, 0.2);
        }


        private void ButtonScaleTweenHover(Button button, float scale, double duration, double revertDuration)
        {
            if (button.ButtonPressed)
                Tween(button, "scale", Vector2.One * ((scale - 1) / 2 + 1), duration);
            else if (button.IsHovered())
                Tween(button, "scale", Vector2.One * scale, duration);
            else
                Tween(button, "scale", Vector2.One, revertDuration);
        }

        private void Tween(GodotObject obj, NodePath property, Variant amount, double duration)
        {
            var tween = CreateTween();
            tween.TweenProperty(obj, property, amount, duration);
        }
    }
}
