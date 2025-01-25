using AquaPapi.Autoload;
using Godot;
using System;

namespace AquaPapi.Scenes
{
    public partial class MainScene : Node2D
    {
        private Global global;
        private StaticBody2D boatStaticBody;

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");
            boatStaticBody = GetNode<StaticBody2D>("Boat");

            global.PlayerGotDropped += OnPlayerDropped;
        }

        private void OnPlayerDropped()
        {
            boatStaticBody.GetNode<CollisionShape2D>("Collision").Disabled = true;
            boatStaticBody.GetNode<ColorRect>("ColorRect").Hide();
        }
    }
}
