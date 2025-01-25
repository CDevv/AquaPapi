using Godot;
using System;

namespace AquaPapi.Components
{
    public partial class Bubble : StaticBody2D
    {
        private AnimatedSprite2D sprite;

        public int Type { get; private set; }

        public override void _Ready()
        {
            sprite = GetNode<AnimatedSprite2D>("Sprite");
        }

        public void SetType(int type)
        {
            if (type <= 1 || type >= 3)
            {
                return;
            }

            Type = type;
            sprite.Frame = type - 1;
        }
    }
}
