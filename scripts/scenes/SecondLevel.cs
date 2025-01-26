using AquaPapi.Abstractions;
using Godot;
using System;

namespace AquaPapi.Scenes
{
    public partial class SecondLevel : BaseScene
    {
        private CanvasLayer shopControl;
        private bool shopAvailable;

        public override void _Ready()
        {
            shopControl = GetNode<CanvasLayer>("ShopControl");

            base._Ready();
            UserInterface.ShowTreats();
            UserInterface.HideMainButtons();
            UserInterface.ToggleStatsUI(true);

            shopControl.Hide();
        }

        public override void _Input(InputEvent @event)
        {
            if (Input.IsActionJustPressed("shopInteractable"))
            {
                if (shopAvailable)
                {
                    Global.CurrentScene.UserInterface.ShowShop();
                }
            }
        }

        private void OnShopAreaEntered(Node2D body)
        {
            shopAvailable = true;
            shopControl.Show();
        }

        private void OnShopAreaExited(Node2D body)
        {
            shopAvailable = false;
            shopControl.Hide();
        }
    }
}
