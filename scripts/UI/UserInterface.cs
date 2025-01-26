using AquaPapi.Autoload;
using Godot;
using System;

namespace AquaPapi.UI
{
    public partial class UserInterface : CanvasLayer
    {
        [Signal]
        public delegate void GameStartedEventHandler();

        private Global global;  // Reference to the global object
        private Button play;
        private Button shop;
        private TextureRect oxygenBack;
        private TextureProgressBar oxygenBar;
        private HealthBar healthBar;
        private NinePatchRect treatsContainer;
        private AnimatedSprite2D treatsIcon;
        private Label treatsLabel;
        private ShopInterface shopInterface;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Get the global object
            global = GetNode<Global>("/root/Global");

            play = GetNode<Button>("PlayButton");
            shop = GetNode<Button>("ShopButton");

            oxygenBack = GetNode<TextureRect>("OxygenBack");
            oxygenBar = GetNode<TextureProgressBar>("OxygenBar");
            healthBar = GetNode<HealthBar>("HealthBar");

            treatsContainer = GetNode<NinePatchRect>("TreatsContainer");
            treatsIcon = GetNode<AnimatedSprite2D>("%TreatsIcon");
            treatsLabel = GetNode<Label>("%TreatsLabel");

            shopInterface = GetNode<ShopInterface>("ShopInterface");

            ToggleStatsUI(false);
        }

        public void ToggleStatsUI(bool toggle)
        {
            if (toggle)
            {
                treatsIcon.Play("default");
            }

            oxygenBack.Visible = toggle;
            oxygenBar.Visible = toggle;
            healthBar.Visible = toggle;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            // Check if the "W" key or any other key action is pressed
            if (Input.IsActionJustPressed("move_up") || Input.IsActionJustPressed("dropPapi"))
            {
                // If the button is hidden (game started), set global.IsInWater to true
                if (!this.Visible) // Button is hidden, so the game has started
                {
                    global.IsInWater = true;
                }

                if (!global.IsInWater)
                {
                    // Start the game logic here (you can add any custom game-start logic you need)
                    StartGame();
                }
            }

            if (Input.IsActionJustPressed("hideShop"))
            {
                shopInterface.Hide();

                if (!global.IsInWater)
                {
                    play.Show();
                    shop.Show();
                }
            }
        }

        public void UpdateInterface()
        {
            treatsLabel.Text = $"{global.Treats}";
            oxygenBar.MaxValue = global.MaxOxygen;
            oxygenBar.Value = global.Oxygen;

            healthBar.SetValue(global.Health, global.MaxHealth);

            global.SaveData();
        }

        private void OnButtonPressed()
        {
            // When the button is pressed, start the game
            StartGame();
        }

        private void StartGame()
        {
            EmitSignal(SignalName.GameStarted);

            ToggleStatsUI(true);
            shopInterface.Hide();

            HideMainButtons();

            global.CurrentScene.Player.PlayAnimation($"falling-{global.SuitLevel+1}");

            // You can add additional game start logic here
            GD.Print("Game Started!");

            // Example: Change the scene to the actual game scene
            // GetTree().ChangeScene("res://Scenes/GameScene.tscn");

            // Set the global variable to true to indicate the game has started
            global.IsInWater = true;
        }

        public void OnShopButton()
        {
            play.Hide();
            shop.Hide();
            shopInterface.Show();
        }

        public void HideMainButtons()
        {
            play.Visible = false;
            shop.Visible = false;
        }

        public void ShowTreats()
        {
            treatsContainer.Show();
        }

        public void ShowShop()
        {
            shopInterface.Show();
        }
    }
}
