using AquaPapi.Autoload;
using Godot;
using System;

namespace AquaPapi.Components
{
    public partial class Player : CharacterBody2D
    {
        public float Speed;
        private float upwardForce = -150f; // Upward force to move the player upwards
        private Global global;

        // Timer to manage jump reset after a period
        private Timer jumpResetTimer;

        // Variable to track if the player can jump
        private bool canJump = true;

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");
            Speed = global.MovementSpeed;

            // Create and set up the jump reset timer
            jumpResetTimer = new Timer();
            AddChild(jumpResetTimer);
            jumpResetTimer.WaitTime = 5.0f; // Reset after 10 seconds
            jumpResetTimer.OneShot = true; // Timer will only trigger once per start
            jumpResetTimer.Autostart = false; // Do not start automatically

            // Connect the timer's timeout signal to the OnJumpResetTimeout method
            jumpResetTimer.Connect("timeout", new Callable(this, nameof(OnJumpResetTimeout)));
        }

        public override void _Input(InputEvent @event)
        {
            // Check if the "dropPapi" action is pressed to trigger the water entry.
            if (Input.IsActionJustPressed("dropPapi"))
            {
                if (!global.IsInWater)
                {
                    global.IsInWater = true;
                }
            }

            // Check if the player presses the "Arrow Up", "W", or "Space" key to move upward
            if ((Input.IsActionJustPressed("ui_up") || Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Space)) && global.IsInWater && canJump)
            {
                // Apply an upward velocity when the key is pressed
                Velocity = new Vector2(Velocity.X, upwardForce);  // Apply upward movement

                // Disable jumping temporarily
                canJump = false;

                // Start the timer to reset jump ability
                jumpResetTimer.Start();
            }
        }

        // Method to reset the jump ability after the timer expires
        private void OnJumpResetTimeout()
        {
            canJump = true;  // Allow jumping again after timeout
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 velocity = Velocity;

            // If in water, reduce gravity to make falling slower
            if (global.IsInWater)
            {
                // Apply reduced gravity (only modify the Y component)
                velocity.Y += GetGravity().Y * 0.1f * (float)delta;  // Slow down falling due to water
            }
            else
            {
                // If not in water, apply normal gravity
                velocity += GetGravity() * (float)delta;
            }

            // Handle movement input on the X-axis
            float inputX = 0;

            // Check for left movement input (using action or key)
            if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("move_left"))
            {
                inputX -= 1;  // Move left
            }
            // Check for right movement input (using action or key)
            if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("move_right"))
            {
                inputX += 1;  // Move right
            }

            // If 'A' or 'D' keys are pressed, move left or right accordingly
            if (Input.IsKeyPressed(Key.A))
            {
                inputX -= 1;  // Move left with 'A'
            }
            if (Input.IsKeyPressed(Key.D))
            {
                inputX += 1;  // Move right with 'D'
            }

            // Apply faster movement on the X-axis if there's input
            if (inputX != 0)
            {
                velocity.X = inputX * Speed * 1.5f;  // Increase speed for horizontal movement
            }
            else
            {
                // If no input, smoothly decelerate to zero
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }

            Velocity = velocity;
            MoveAndSlide();

            // Keep the camera's position synced with the player's position
            global.CurrentScene.Camera.Position = Position;
        }
    }
}
