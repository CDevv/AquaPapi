using AquaPapi.Autoload;
using Godot;
using System;

namespace AquaPapi.Components
{
    public partial class Player : CharacterBody2D
    {
        public float Speed;
        private float upwardForce = -80f; // Reduced force for the initial jump
        private float jumpMultiplier = 1.0f; // Jump multiplier
        private Global global;

        // Timer to manage jump reset after a period
        private Timer jumpResetTimer;

        // Variable to track if the player can jump
        private bool canJump = true;
        private bool hasJumped = false; // Check if the player has already jumped

        private AnimatedSprite2D sprite;

        public override void _Ready()
        {
            global = GetNode<Global>("/root/Global");
            sprite = GetNode<AnimatedSprite2D>("Sprite");
            Speed = global.MovementSpeed;

            // Create and set up the jump reset timer
            jumpResetTimer = new Timer();
            AddChild(jumpResetTimer);
            jumpResetTimer.WaitTime = 5.0f; // Reset after 5 seconds
            jumpResetTimer.OneShot = true; // Timer triggers only once
            jumpResetTimer.Autostart = false; // Do not start automatically

            // Connect the timer's timeout signal to the OnJumpResetTimeout method
            jumpResetTimer.Connect("timeout", new Callable(this, nameof(OnJumpResetTimeout)));

            sprite.Play($"idle-{global.SuitLevel+1}");

            GD.Print(global.CurrentScene == null);
        }

        public override void _Input(InputEvent @event)
        {
            // Check if the player presses the "Arrow Up", "W", or "Space" key to move upward
            if ((Input.IsActionJustPressed("ui_up") || Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Space)) && global.IsInWater && canJump)
            {
                if (!hasJumped) // If this is the first jump
                {
                    upwardForce = -60f; // Reduced force for the first jump
                    jumpMultiplier = 1.0f; // No multiplier for the first jump
                }
                else // After the initial jump
                {
                    upwardForce = -80f; // Increased force for subsequent jumps
                    jumpMultiplier = 1.5f; // Increase jump strength
                }

                // Apply upward velocity
                Velocity = new Vector2(Velocity.X, upwardForce * jumpMultiplier);

                // Temporarily disable jumping
                canJump = false;

                // Start the timer to reset jump ability
                jumpResetTimer.Start();

                hasJumped = true; // Mark that the player has jumped
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

            // If in water, apply a stronger gravity effect to avoid gliding
            if (global.IsInWater)
            {
                velocity.Y += (GetGravity().Y / 3) * 0.1f * (float)delta;  // Increased gravity in water to avoid gliding
            }
            else
            {
                velocity += GetGravity() * (float)delta;
            }

            // Handle movement input on the X-axis
            float inputX = 0;

            if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("move_left"))
            {
                inputX -= 1;  // Move left
            }
            if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("move_right"))
            {
                inputX += 1;  // Move right
            }

            if (Input.IsKeyPressed(Key.A))
            {
                inputX -= 1;  // Move left with 'A'
                sprite.FlipH = true;
            }
            if (Input.IsKeyPressed(Key.D))
            {
                inputX += 1;  // Move right with 'D'
                sprite.FlipH = false;
            }

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

        public async void OnMineCollided()
        {
            global.Health -= 1;
            GD.Print("Health: ", global.Health);

            global.CurrentScene.UserInterface.UpdateInterface();

            sprite.Play($"hurt-{global.SuitLevel+1}");
            await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);
            sprite.Play($"falling-{global.SuitLevel+1}");
        }

        public void PlayAnimation(string name)
        {
            sprite.Play(name);
        }

        public void ChangeSuit(int suit)
        {
            sprite.Play($"idle-{suit+1}");
        }
    }
}
