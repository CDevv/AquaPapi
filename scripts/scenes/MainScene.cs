using AquaPapi.Abstractions;
using AquaPapi.Autoload;
using AquaPapi.Components;
using Godot;
using System;

namespace AquaPapi.Scenes
{
    public partial class MainScene : BaseScene
    {
        private StaticBody2D boatStaticBody;

        public override void _Ready()
        {
            base._Ready();
            boatStaticBody = GetNode<StaticBody2D>("Boat");

            Global.PlayerGotDropped += OnPlayerDropped;
        }

        private void OnPlayerDropped()
        {
            boatStaticBody.GetNode<CollisionShape2D>("Collision").Disabled = true;
            boatStaticBody.GetNode<ColorRect>("ColorRect").Hide();
        }
    }
}
