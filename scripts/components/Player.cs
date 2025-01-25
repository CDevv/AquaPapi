using Godot;
using System;
using AquaPapi.Autoload;

namespace AquaPapi.Components
{
    public partial class Player : CharacterBody2D
    {
        public const float Speed = 300.0f;
        public const float JumpVelocity = -400.0f;

        private Global global;

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");
        }

        public override void _Input(InputEvent @event)
        {
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

            // Add the gravity.
            if (!IsOnFloor())
            {
                velocity += GetGravity() * (float)delta;
            }

            // Handle Jump.
            if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            {
                velocity.Y = JumpVelocity;
            }

            // Get the input direction and handle the movement/deceleration.
            // Check for movement input (a, d, left, right)
            float inputX = 0;

            if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("move_left"))
            {
                inputX -= 1;  // Move left
            }
            if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("move_right"))
            {
                inputX += 1;  // Move right
            }

            // If 'a' and 'd' keys are also desired
            if (Input.IsKeyPressed(Key.A))  // Correct key check for 'A'
            {
                inputX -= 1;  // Move left with 'A'
            }
            if (Input.IsKeyPressed(Key.D))  // Correct key check for 'D'
            {
                inputX += 1;  // Move right with 'D'
            }

            // Apply movement to the X-axis
            if (inputX != 0)
            {
                velocity.X = inputX * Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }

            Velocity = velocity;
            MoveAndSlide();

            global.CurrentScene.Camera.Position = Position;
        }
    }
}
