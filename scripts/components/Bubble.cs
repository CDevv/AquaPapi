using AquaPapi.Autoload;
using Godot;
using System;

namespace AquaPapi.Components
{
    public partial class Bubble : Node2D
    {
        [Signal]
        public delegate void CollidedEventHandler(float value);

        private Global global;
        private AnimatedSprite2D sprite;
        private AudioStreamPlayer streamPlayer;

        public int Type { get; private set; }
        public float Value { get; set; }

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");
            sprite = GetNode<AnimatedSprite2D>("Sprite");
            streamPlayer = GetNode<AudioStreamPlayer>("Sound");
        }

        public void SetType(int type)
        {
            if (type <= 1 || type >= 3)
            {
                return;
            }

            Type = type;
            sprite.Frame = type;
            Value = global.BubblesInfo[global.Level][type][0];
        }

        private void OnBodyEntered(Node2D body)
        {
            EmitSignal(SignalName.Collided, Value);
            QueueFree();
            streamPlayer.Play();
        }
    }
}
