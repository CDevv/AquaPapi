using AquaPapi.Autoload;
using Godot;
using System;

public partial class UserInterface : CanvasLayer
{
    private Global global;  // Reference to the global object
    private Button play;
    private Button shop;
    private TextureRect oxygenBack;
    private TextureProgressBar oxygenBar;
    private NinePatchRect treatsContainer;
    private AnimatedSprite2D treatsIcon;
    private Label treatsLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get the global object
        global = GetNode<Global>("/root/Global");

        play = GetNode<Button>("PlayButton");
        shop = GetNode<Button>("ShopButton");

        oxygenBack = GetNode<TextureRect>("OxygenBack");
        oxygenBar = GetNode<TextureProgressBar>("OxygenBar");

        treatsContainer = GetNode<NinePatchRect>("TreatsContainer");
        treatsIcon = GetNode<AnimatedSprite2D>("%TreatsIcon");
        treatsLabel = GetNode<Label>("%TreatsLabel");

        ToggleStatsUI(false);
    }

    private void ToggleStatsUI(bool toggle)
    {
        if (toggle)
        {
            treatsIcon.Play("default");
        }

        oxygenBack.Visible = toggle;
        oxygenBar.Visible = toggle;
        treatsContainer.Visible = toggle;
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

            // Start the game logic here (you can add any custom game-start logic you need)
            StartGame();
        }
    }

    public void UpdateInterface()
    {
        treatsLabel.Text = $"{global.Treats}";
        oxygenBar.MaxValue = global.MaxOxygen;
        oxygenBar.Value = global.Oxygen;
    }

    private void OnButtonPressed()
    {
        // When the button is pressed, start the game
        StartGame();
    }

    private void StartGame()
    {
        ToggleStatsUI(true);

        play.Visible = false;
        shop.Visible = false;

        // You can add additional game start logic here
        GD.Print("Game Started!");

        // Example: Change the scene to the actual game scene
        // GetTree().ChangeScene("res://Scenes/GameScene.tscn");

        // Set the global variable to true to indicate the game has started
        global.IsInWater = true;
    }
}
