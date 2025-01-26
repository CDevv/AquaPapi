using AquaPapi.Abstractions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace AquaPapi.Autoload
{
    public partial class Global : Node
    {
        [Signal]
        public delegate void PlayerGotDroppedEventHandler();

        public BaseScene CurrentScene { get; set; }

        private bool isInWater;

        public bool IsInWater
        {
            get { return isInWater; }
            set 
            { 
                isInWater = value;
                if (value)
                {
                    EmitSignal(SignalName.PlayerGotDropped);
                }
            }
        }

        public Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<int, Godot.Collections.Array<float>>> GarbageInfo { get; private set; }
        public Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<int, Godot.Collections.Array<float>>> BubblesInfo { get; set; }
        public Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<int, Godot.Collections.Array<float>>> UpgradesInfo { get; set; }
        public RandomNumberGenerator Random { get; private set; }

        public int Treats { get; set; }
        public int Health { get; set; } = 10;
        public int MaxHealth { get; set; } = 10;
        public int Oxygen { get; set; } = 5;
        public int MaxOxygen { get; set; } = 5;
        public float MovementSpeed { get; set; } = 100.0f;
        public int Level { get; set; } = 1;
        public int SuitLevel { get; set; } = 0;
        public int OxygenLevel { get; set; } = 0;
        public int HealthLevel { get; set; } = 0;

        public override void _Ready()
        {
            LoadCursor();

            GarbageInfo = new();
            BubblesInfo = new();
            UpgradesInfo = new();
            Random = new RandomNumberGenerator();
            GetGarbageInfo();
            GetBubblesInfo();
            GetUpgradesInfo();
        }



        private void LoadCursor()
        {
            var arrow = ResourceLoader.Load("res://assets/cursor_normal.png");
            var arrowHover = ResourceLoader.Load("res://assets/cursor_hover.png");

            Input.SetCustomMouseCursor(arrow);
            Input.SetCustomMouseCursor(arrowHover, Input.CursorShape.PointingHand);
        }

        private void GetGarbageInfo()
        {
            FileAccess garbagesFile = FileAccess.Open("res://info/garbage.json", FileAccess.ModeFlags.Read);
            string garbagesJson = garbagesFile.GetAsText();
            garbagesFile.Close();

            Godot.Collections.Dictionary<string, Variant> garbagesDict =
                (Godot.Collections.Dictionary<string, Variant>)Json.ParseString(garbagesJson);

            foreach (var item in garbagesDict)
            {
                Godot.Collections.Dictionary<int, Godot.Collections.Array<float>> typeValues = new();
                Godot.Collections.Dictionary<Variant, Variant> itemData =
                    (Godot.Collections.Dictionary<Variant, Variant>)item.Value;

                foreach (var garbageType in itemData)
                {
                    int typeKey = int.Parse((string)garbageType.Key);
                    Godot.Collections.Array<float> typeValue = 
                        (Godot.Collections.Array<float>)garbageType.Value;

                    typeValues[typeKey] = typeValue;
                }

                int key = int.Parse(item.Key);
                GarbageInfo[key] = typeValues;
            }
        }

        private void GetBubblesInfo()
        {
            FileAccess bubblesFile = FileAccess.Open("res://info/bubbles.json", FileAccess.ModeFlags.Read);
            string bubblesJson = bubblesFile.GetAsText();
            bubblesFile.Close();

            Godot.Collections.Dictionary<string, Variant> garbagesDict =
                (Godot.Collections.Dictionary<string, Variant>)Json.ParseString(bubblesJson);

            foreach (var item in garbagesDict)
            {
                Godot.Collections.Dictionary<int, Godot.Collections.Array<float>> typeValues = new();
                Godot.Collections.Dictionary<Variant, Variant> itemData =
                    (Godot.Collections.Dictionary<Variant, Variant>)item.Value;

                foreach (var garbageType in itemData)
                {
                    int typeKey = int.Parse((string)garbageType.Key);
                    Godot.Collections.Array<float> typeValue =
                        (Godot.Collections.Array<float>)garbageType.Value;

                    typeValues[typeKey] = typeValue;
                }

                int key = int.Parse(item.Key);
                BubblesInfo[key] = typeValues;
            }
        }

        private void GetUpgradesInfo()
        {
            FileAccess upgradesFile = FileAccess.Open("res://info/upgrades.json", FileAccess.ModeFlags.Read);
            string upgradesJson = upgradesFile.GetAsText();
            upgradesFile.Close();

            Godot.Collections.Dictionary<string, Variant> upgradesDict =
                (Godot.Collections.Dictionary<string, Variant>)Json.ParseString(upgradesJson);

            foreach (var item in upgradesDict)
            {
                Godot.Collections.Dictionary<int, Godot.Collections.Array<float>> typeValues = new();
                Godot.Collections.Dictionary<Variant, Variant> itemData =
                    (Godot.Collections.Dictionary<Variant, Variant>)item.Value;

                foreach (var upgradeType in itemData)
                {
                    int typeKey = int.Parse((string)upgradeType.Key);
                    Godot.Collections.Array<float> typeValue =
                        (Godot.Collections.Array<float>)upgradeType.Value;

                    typeValues[typeKey] = typeValue;
                }

                UpgradesInfo[item.Key] = typeValues;
            }
        }
    }
}
