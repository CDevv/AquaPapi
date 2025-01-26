using AquaPapi.Autoload;
using AquaPapi.Components;
using AquaPapi.UI;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaPapi.Abstractions
{
    public partial class BaseScene : Node2D
    {
        [Export]
        public int GenCount { get; set; } = 25;
        [Export]
        public int ObstacleGenCount { get; set; } = 5;
        [Export]
        public Array<string> Obstacles { get; set; }

        protected Global Global { get; set; }
        public Timer OxygenTimer { get; set; }
        public Timer ObstacleTimer { get; set; }
        public Player Player { get; set; }
        public Camera2D Camera { get; protected set; }
        public Area2D WaterArea { get; private set; }
        public CollisionShape2D AreaShape { get; private set; }
        public UserInterface UserInterface { get; private set; }

        [Export]
        public PackedScene GarbagePackedScene { get; private set; }
        [Export]
        public PackedScene BubblePackedScene { get; private set; }
        [Export]
        public PackedScene ObstaclePackedScene { get; private set; }

        public override void _Ready()
        {
            Global = GetNode<Global>("/root/Global");
            OxygenTimer = GetNode<Timer>("OxygenTimer");
            ObstacleTimer = GetNode<Timer>("ObstacleTimer");
            Player = GetNode<Player>("Player");
            Camera = GetNode<Camera2D>("Camera");
            WaterArea = GetNode<Area2D>("WaterArea");
            AreaShape = GetNode<CollisionShape2D>("%WaterShape");
            UserInterface = GetNodeOrNull<UserInterface>("UserInterface");

            ObstacleTimer.Start();

            Global.CurrentScene = this;

            GenerateGarbage();
            GenerateBubbles();

            UserInterface.UpdateInterface();
        }

        public void GenerateGarbage()
        {
            Vector2 rectStart = WaterArea.Position;
            Vector2 rectEnd = WaterArea.Position + AreaShape.Shape.GetRect().Size;
            int level = Global.Level;
            int[] types = Global.GarbageInfo[level].Keys.ToArray();
            float[] values = Global.GarbageInfo[level].Values.Select(x => x[1]).ToArray();

            for (int i = 0; i < GenCount; i++)
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

            for (int i = 0; i < GenCount; i++)
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

        private void GenerateObstacles()
        {
            float minY = 0;
            float maxY = AreaShape.Shape.GetRect().Size.Y;
            float minX = 0;
            float maxX = AreaShape.Shape.GetRect().Size.X;

            //string[] obstacleTypes = Global.ObstaclesInfo.Keys.ToArray();
            string[] obstacleTypes = Obstacles.ToArray();
            GD.Print(obstacleTypes);

            for (int i = 0; i < ObstacleGenCount; i++)
            {
                float targetY = (float)GD.RandRange(minY, maxY);

                Vector2 position = new Vector2(maxX - 20, WaterArea.Position.Y + targetY);

                FishObstacle fish = ObstaclePackedScene.Instantiate<FishObstacle>();
                fish.Position = position;

                int chosenTypeIndex = GD.RandRange(0, obstacleTypes.Length - 1);
                string chosenType = obstacleTypes[chosenTypeIndex];

                double randomBoolDouble = GD.RandRange(0, 1);
                bool randomBool = randomBoolDouble == 0 ? false : true;

                fish.OtherSide = randomBool;

                if (randomBool)
                {
                    position = new Vector2(10, WaterArea.Position.Y + targetY);
                    fish.Position = position;
                }

                AddChild(fish);
                fish.SetType(chosenType);
            }
        }

        private void OnGarbageCollision(float value)
        {
            Global.Treats += (int)value;
            GD.Print("Treats:", Global.Treats);

            UserInterface.UpdateInterface();
        }

        private void OnBubbleCollision(float value)
        {
            int oxygenChange = (int)Mathf.Clamp(value, 0, Global.MaxOxygen - Global.Oxygen);
            Global.Oxygen += oxygenChange;
            GD.Print("Oxygen: ", Global.Oxygen);

            UserInterface.UpdateInterface();
        }

        private void OnOxygenTimer()
        {
            if (Global.IsInWater)
            {
                Global.Oxygen -= 1;
                UserInterface.UpdateInterface();
            }
        }

        private void OnObstacleTimer()
        {
            GenerateObstacles();
        }
    }
}
