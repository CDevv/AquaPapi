using AquaPapi.Abstractions;
using AquaPapi.Autoload;
using Godot;
using System;

[Tool]
public partial class Mine : Node2D
{
	[Signal]
	public delegate void CollidedEventHandler();

	private int type;
    [Export]
	public int Type
	{
		get { return type; }
		set
		{
			type = value;
            if (type == 0 || type == 1)
            {
				GetNode<AnimatedSprite2D>("Sprite").Frame = type;
            }
        }
	}

	private Global global;
    private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		sprite = GetNode<AnimatedSprite2D>("Sprite");

		sprite.Frame = Type;
	}

	private void OnBodyEntered(Node2D body)
	{
		EmitSignal(SignalName.Collided);

		global.CurrentScene.Player.OnMineCollided();
	}
}
