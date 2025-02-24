using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Riftstrike.src.units;

namespace Riftstrike.src
{
    public partial class DataManager : Node
    {
        public static DataManager Instance { get; private set; }

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null)
            {
                QueueFree();
                return;
            }
            Instance = this;
        }

        public Array<UnitData> UnitData = new();

        public static void OverwriteUnitData(IEnumerable<UnitData> data)
        {
            Instance.UnitData.Clear();
            Instance.UnitData.AddRange(data);
        }

        public static void DeleteUnitData()
        {
            Instance.UnitData.Clear();
        }
    }
}