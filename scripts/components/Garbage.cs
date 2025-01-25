using AquaPapi.Autoload;
using Godot;
using System;

namespace AquaPapi.Components
{
    public partial class Garbage : Node2D
    {
        [Signal]
        public delegate void CollidedEventHandler(float value);

        private Global global;
        private AnimatedSprite2D sprite;
        public float Value { get; set; }

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");
            sprite = GetNode<AnimatedSprite2D>("Sprite");
        }

        public void SetType(int type)
        {
            sprite.Frame = type;
            Value = global.GarbageInfo[global.Level][type][0];
        }

        private void OnBodyEntered(Node2D body)
        {
            EmitSignal(SignalName.Collided, Value);
            QueueFree();
        }
    }
}
