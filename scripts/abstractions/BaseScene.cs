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

            for (int i = 0; i < 10; i++)
            {
                float targetX = (float)GD.RandRange(rectStart.X, rectEnd.X);
                float targetY = (float)GD.RandRange(rectStart.Y, rectEnd.Y);
                Vector2 targetPos = new Vector2(targetX, targetY);

                Garbage garbage = GarbagePackedScene.Instantiate<Garbage>();

                GetNode<Node>("GarbageContainer").AddChild(garbage);
                garbage.Position = targetPos;
            }
        }

        public void GenerateBubbles()
        {
            Vector2 rectStart = WaterArea.Position;
            Vector2 rectEnd = WaterArea.Position + AreaShape.Shape.GetRect().Size;

            for (int i = 0; i < 10; i++)
            {
                float targetX = (float)GD.RandRange(rectStart.X, rectEnd.X);
                float targetY = (float)GD.RandRange(rectStart.Y, rectEnd.Y);
                Vector2 targetPos = new Vector2(targetX, targetY);

                Bubble bubble = BubblePackedScene.Instantiate<Bubble>();

                GetNode<Node>("BubblesContainer").AddChild(bubble);
                bubble.Position = targetPos;
            }
        }
    }
}
