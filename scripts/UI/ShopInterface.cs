using AquaPapi.Autoload;
using Godot;
using System;
using System.Diagnostics;

namespace AquaPapi.UI
{
    public partial class ShopInterface : CanvasLayer
    {
        private Global global;

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");

            UpdateLabels();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        private void UpdateLabels()
        {
            if (global.SuitLevel <= 3)
            {
                if (global.SuitLevel < 3)
                {
                    int price = (int)global.UpgradesInfo["Suit"][global.SuitLevel + 1][1];
                    GetNode<Label>("%SuitLabel").Text = $"{price}";

                    GetNode<AnimatedSprite2D>("%SuitIcon").Frame = global.SuitLevel;
                    GetNode<AnimatedSprite2D>("%SuitUpgradeBar").Frame = global.SuitLevel + 1;
                }
                else
                {
                    GetNode<Label>("%SuitLabel").Text = "MAX";
                    GetNode<AnimatedSprite2D>("%SuitIcon").Frame = global.SuitLevel;
                    GetNode<AnimatedSprite2D>("%SuitUpgradeBar").Frame = 4;
                }
            }

            if (global.HealthLevel <= 3)
            {
                if (global.HealthLevel < 3)
                {
                    int price = (int)global.UpgradesInfo["Health"][global.HealthLevel + 1][1];
                    GetNode<Label>("%HealthLabel").Text = $"{price}";

                    GetNode<AnimatedSprite2D>("%HealthIcon").Frame = global.HealthLevel;
                    GetNode<AnimatedSprite2D>("%HealthUpgradeBar").Frame = global.HealthLevel + 1;
                }
                else
                {
                    GetNode<Label>("%HealthLabel").Text = "MAX";
                    GetNode<AnimatedSprite2D>("%HealthIcon").Frame = global.HealthLevel;
                    GetNode<AnimatedSprite2D>("%HealthUpgradeBar").Frame = 4;
                }
            }

            if (global.OxygenLevel <= 3)
            {
                if(global.OxygenLevel < 3)
                {
                    int price = (int)global.UpgradesInfo["Oxygen"][global.OxygenLevel + 1][1];
                    GetNode<Label>("%OxygenLabel").Text = $"{price}";

                    GetNode<AnimatedSprite2D>("%OxygenIcon").Frame = global.OxygenLevel;
                    GetNode<AnimatedSprite2D>("%OxygenUpgradeBar").Frame = global.OxygenLevel + 1;
                }
                else
                {
                    GetNode<Label>("%OxygenLabel").Text = "MAX";

                    GetNode<AnimatedSprite2D>("%OxygenIcon").Frame = global.OxygenLevel;
                    GetNode<AnimatedSprite2D>("%OxygenUpgradeBar").Frame = 4;
                }
            }
        }

        private void OnSuitButton()
        {
            if (global.SuitLevel < 3)
            {
                GD.Print(global.SuitLevel);
                int price = (int)global.UpgradesInfo["Suit"][global.SuitLevel + 1][1];
                if (global.Treats >= price)
                {
                    global.SuitLevel++;
                    global.Treats -= price;

                    UpdateLabels();
                    global.CurrentScene.UserInterface.UpdateInterface();
                    global.CurrentScene.Player.ChangeSuit(global.SuitLevel);
                }
            }
        }

        private void OnHealthButton()
        {
            if (global.HealthLevel <= 3)
            {
                int price = (int)global.UpgradesInfo["Health"][global.HealthLevel + 1][1];
                if (global.Treats >= price)
                {
                    global.HealthLevel++;
                    global.Treats -= price;

                    UpdateLabels();

                    int newHealth = (int)global.UpgradesInfo["Health"][global.HealthLevel][0];

                    global.Health = newHealth;
                    global.MaxHealth = newHealth;

                    global.CurrentScene.UserInterface.UpdateInterface();
                }
            }
        }

        private void OnOxygenButton()
        {
            if (global.OxygenLevel <= 3)
            {
                int price = (int)global.UpgradesInfo["Oxygen"][global.OxygenLevel + 1][1];
                if (global.Treats >= price)
                {
                    global.OxygenLevel++;
                    global.Treats -= price;

                    UpdateLabels();

                    int newOxygen = (int)global.UpgradesInfo["Oxygen"][global.OxygenLevel][0];

                    global.Oxygen = newOxygen;
                    global.MaxOxygen = newOxygen;

                    global.CurrentScene.UserInterface.UpdateInterface();
                }
            }
        }
    }
}
