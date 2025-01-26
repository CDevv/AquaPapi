using AquaPapi.Autoload;
using Godot;
using System;

public partial class FishObstacle : Area2D
{
	[Signal]
	public delegate void CollidedEventHandler();

	private Global global;
	private AnimatedSprite2D sprite;

    public string Type { get; set; }
    public int Damage { get; set; } = 10;
	public int Speed { get; set; } = 10;
    public bool OtherSide { get; set; }

    public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		sprite = GetNode<AnimatedSprite2D>("Sprite");

        if (OtherSide)
        {
			sprite.FlipH = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
		int change = OtherSide ? Speed : -Speed;

		Vector2 velocity = new Vector2(change, 0);

		Position += velocity;
    }

    public void SetType(string type)
	{
        GD.Print(sprite == null);
		Type = type;
		sprite.Play(type);
		Damage = global.ObstaclesInfo[type]["Damage"];
        Speed = global.ObstaclesInfo[type]["Speed"];

        GetNode<CollisionShape2D>(type).Disabled = false;
	}

	private void OnCollision(Node2D body)
	{
		GD.Print("obstacle");
		EmitSignal(SignalName.Collided);

		global.Health -= (Damage / global.MaxHealth) * 100;
        GetNode<CollisionShape2D>(Type).Disabled = true;

		global.CurrentScene.UserInterface.UpdateInterface();
    }

	private void OnScreenExited()
	{
		QueueFree();
	}
}
