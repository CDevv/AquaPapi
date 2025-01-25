using AquaPapi.Autoload;
using AquaPapi.Components;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaPapi.Abstractions
{
    public partial class BaseScene : Node2D
    {
        protected Global Global { get; set; }
        public Camera2D Camera { get; protected set; }
        public Area2D WaterArea { get; private set; }
        public CollisionShape2D AreaShape { get; private set; }

        [Export]
        public PackedScene GarbagePackedScene { get; private set; }
        [Export]
        public PackedScene BubblePackedScene { get; private set; }

        public override void _Ready()
        {
            Global = GetNode<Global>("/root/Global");
            Camera = GetNode<Camera2D>("Camera");
            WaterArea = GetNode<Area2D>("WaterArea");
            AreaShape = GetNode<CollisionShape2D>("%WaterShape");

            Global.CurrentScene = this;

            GenerateGarbage();
            GenerateBubbles();
        }

        public void GenerateGarbage()
        {
            Vector2 rectStart = WaterArea.Position;
            Vector2 rectEnd = WaterArea.Position + AreaShape.Shape.GetRect().Size;
            int level = Global.Level;
            int[] types = Global.GarbageInfo[level].Keys.ToArray();
            float[] values = Global.GarbageInfo[level].Values.Select(x => x[1]).ToArray();

            for (int i = 0; i < 25; i++)
            {
                float targetX = (float)GD.RandRange(rectStart.X, rectEnd.X);
                float targetY = (float)GD.RandRange(rectStart.Y, rectEnd.Y);
                Vector2 targetPos = new Vector2(targetX, targetY);

                Garbage garbage = GarbagePackedScene.Instantiate<Garbage>();

                int chosenType = types[Global.Random.RandWeighted(values)];

                GetNode<Node>("GarbageContainer").AddChild(garbage);
                garbage.Position = targetPos;
                garbage.SetType(chosenType);

                garbage.Collided += OnGarbageCollision;
            }
        }

        public void GenerateBubbles()
        {
            Vector2 rectStart = WaterArea.Position;
            Vector2 rectEnd = WaterArea.Position + AreaShape.Shape.GetRect().Size;
            int level = Global.Level;
            int[] types = Global.BubblesInfo[level].Keys.ToArray();
            float[] values = Global.BubblesInfo[level].Values.Select(x => x[1]).ToArray();

            for (int i = 0; i < 25; i++)
            {
                float targetX = (float)GD.RandRange(rectStart.X, rectEnd.X);
                float targetY = (float)GD.RandRange(rectStart.Y, rectEnd.Y);
                Vector2 targetPos = new Vector2(targetX, targetY);

                Bubble bubble = BubblePackedScene.Instantiate<Bubble>();

                int chosenType = types[Global.Random.RandWeighted(values)];

                GetNode<Node>("BubblesContainer").AddChild(bubble);
                bubble.Position = targetPos;
                bubble.SetType(chosenType);

                bubble.Collided += OnBubbleCollision;
            }
        }

        private void OnGarbageCollision(float value)
        {
            Global.Treats += (int)value;
            GD.Print("Treats:", Global.Treats);
        }

        private void OnBubbleCollision(float value)
        {
            int oxygenChange = (int)Mathf.Clamp(value, 0, Global.MaxOxygen - Global.Oxygen);
            Global.Oxygen += oxygenChange;
            GD.Print("Oxygen: ", Global.Oxygen);
        }
    }
}
