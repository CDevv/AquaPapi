using AquaPapi.Autoload;
using Godot;
using System;

public partial class Button : Godot.Button
{
    private Global global;  // Variable for the global object

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get the global object
        global = GetNode<Global>("/root/Global");

        // Connect the button's pressed signal to a method
        this.Pressed += OnButtonPressed;
    }

    public override void _Process(double delta)
    {
        // Check if the "W" key or any other key action is pressed
        if (Input.IsActionJustPressed("move_up") || Input.IsKeyPressed(Key.W))
        {
            // If the button is hidden (game started), set global.IsInWater to true
            if (!this.Visible) // Button is hidden, so the game has started
            {
                global.IsInWater = true;
            }
        }

        // Hide the button when IsInWater becomes true
        if (global.IsInWater)
        {
            this.Visible = false;
        }
    }

    private void OnButtonPressed()
    {
        // When the button is pressed, start the game
        StartGame();
    }

    private void StartGame()
    {
        // Hide the button and start the game logic
        this.Visible = false;

        // You can add additional game start logic here
        GD.Print("Game Started!");

        // Example: Change the scene to the actual game scene
        // GetTree().ChangeScene("res://Scenes/GameScene.tscn");

        // Set the global variable to true
        global.IsInWater = true;
    }
}
