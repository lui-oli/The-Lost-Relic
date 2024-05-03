using Godot;
using System;
using System.ComponentModel;

public partial class player : CharacterBody2D
{
	public enum PlayerMode{
		SMALL,
		BIG,
		SHOOTING
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Godot Export Reference
	
	private PlayerAnimatedSprite animatedSprite2D; // At the class level as a private field. 

	// Godot Export References
	[Export]
	private float runSpeedDamping = 0.5f;
	[Export]
	// !max_walk = $14 = 20 = 20subpixels / frame = 1.25 pixels / frame = 75 pixels per second
	private const float Speed = 75.0f;
	[Export]
	private const float JumpVelocity = -400.0f;

    public override void _Ready()
    {
		// For auto complete hints
		animatedSprite2D = GetNode<PlayerAnimatedSprite>("AnimatedSprite2D"); // GetNode<T>("NodePath") used to fetch a node from the scene tree. (as PlayerAnimatedSprite = generic type parameter <T>)
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;  
		var playerMode = PlayerMode.SMALL;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta; // Cast delta to float   

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Holding Jump Mechanic
		if (Input.IsActionJustReleased("ui_accept") && velocity.Y < 0)
			velocity.Y *= 0.5f; // Float literal

		// Handle inertial movement
		var direction = Input.GetAxis("ui_left", "ui_right");

		if (direction != 0)
		// Linear interpolaiton (Lerp(start, end, percentage))
			velocity.X = Mathf.Lerp(velocity.X, Speed * direction, runSpeedDamping * (float)delta);
		else
		{
			// Move towards zero
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed * (float)delta);
		}

		animatedSprite2D.TriggerAnimation(velocity, direction, playerMode);

		Velocity = velocity;
		MoveAndSlide();
	}
}
