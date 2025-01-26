using Godot;
using System;

namespace AquaPapi.UI
{
    public partial class HealthBar : Node2D
    {
        private TextureRect healthBack;
        private TextureProgressBar healthProgressBar;
        private AnimatedSprite2D sprite;

        public override void _Ready()
        {
            healthBack = GetNode<TextureRect>("HealthBack");
            healthProgressBar = GetNode<TextureProgressBar>("ProgressBar");
            sprite = GetNode<AnimatedSprite2D>("Sprite");

            sprite.Play("idle");
        }

        public async void SetValue(int value, int maxValue)
        {
            healthProgressBar.Value = value;
            healthProgressBar.MaxValue = maxValue;

            sprite.Play("angry");
            await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);
            sprite.Play("idle");
        }
    }

}