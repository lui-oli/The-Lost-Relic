using Godot;
using System;
using System.Net;
using System.Runtime.CompilerServices;

public partial class PlayerAnimatedSprite : AnimatedSprite2D
{
	private CharacterBody2D playerCharacterBody;
	private Node2D player2D;

    public void TriggerAnimation(Vector2 velocity, float direction, player.PlayerMode playerMode)
	{
		// Fetch the Animation. We are either SMALL, BIG or SHOTTING.
		string AnimationPrefix = playerMode.ToString();
		
		if (!playerCharacterBody.IsOnFloor())
		{
			Play($"{AnimationPrefix}Jump");
		}
		// If the current velocity has a different vector than current direction then play slide animation because we want to go the other way
		else if (Mathf.Sign(velocity.X) != Mathf.Sign(direction) && velocity.X != 0 && direction != 0)  
		{
			Play($"{AnimationPrefix}Slide");
			// Update the entire Scale propety to get a reference to the Scale method so I an directly modify the Scale.X or Scale.Y properties. 
			Scale = new Vector2(direction, Scale.Y);
		}  	
		// This handles the Sprite direction
		else {
			// Used for flipping the sprite based on direction. When changing the direction the sprite is facing, we create a new Vector2 with the x-component negated. Settings the entire Scale vecotr.
			if ( Mathf.Sign( velocity.X ) != Mathf.Sign(Scale.X) && velocity.X != 0 ) {
				Scale = new Vector2(-Scale.X, Scale.Y);
			}

			// This handles the Run and Idle Animations
			if (velocity.X != 0) {
				Play($"{AnimationPrefix}Run");
			}
			else{
				Play($"{AnimationPrefix}Idle");
			}
		}
	}
}

