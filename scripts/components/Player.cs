using AquaPapi.Autoload;
using Godot;
using System;

namespace AquaPapi.Components
{
    public partial class Player : CharacterBody2D
    {
        public float Speed;

        private Global global;

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");
            Speed = global.MovementSpeed;
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
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 velocity = Velocity;

            // If in water, reduce gravity to make falling slower
            if (global.IsInWater)
            {
                // Apply reduced gravity (only modify the Y component)
                velocity.Y += GetGravity().Y * 0.1f * (float)delta;  // Reducing gravity on Y-axis
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
