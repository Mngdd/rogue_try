using Godot;
using System;
using System.Diagnostics;

// оно автоматом сделало partial class, так что мб 
// будет хорошо если тут будет лежать только мувмент игрока??
public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public const float GlidingRatio = 0.08f;
	public const float TurningRatio = 0.30f;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	// _PhysicsProcess в отличие от _Process гарантирует стабильное обновление
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;
		
		ProcessVerticalMovement(ref velocity, ref delta);
		ProcessHorisontalMovement(ref velocity);

		Velocity = velocity;
		MoveAndSlide(); // обновляет велосити как я понял...
	}

	// Processes jump and gravity mechanics
	private void ProcessVerticalMovement (ref Vector2 velocity, ref double delta) {
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("move_up") && IsOnFloor())
			velocity.Y = JumpVelocity;
	}

	// Processes player horisontal movement
	private void ProcessHorisontalMovement(ref Vector2 velocity) {
		// Get the input direction and handle the movement/deceleration.
		// https://docs.godotengine.org/en/4.2/classes/class_input.html#class-input-method-get-vector
		Vector2 direction = Input.GetVector(
			"move_left", "move_right", "move_up", "move_down"
			);
		// Changes velocity if there's horisontal movement
		if ( direction.X != 0) {
			Turn(ref direction, ref velocity);
		}
		// Returns velocity to base value if there's no horisontal input
		else {
			Glide(ref velocity);
		}
	}

	// Slowly sets velocity to zero if movement keys are not pressed and character is on floor
	private void Glide(ref Vector2 velocity) {
		if (IsOnFloor()) {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, GlidingRatio*Speed);
		}
	}
	// Slowly sets velocity to the value that depend on direction inputed by the player
	private void Turn(ref Vector2 direction, ref Vector2 velocity) {
		velocity.X = Mathf.MoveToward(Velocity.X, direction.X*Speed, TurningRatio*Speed);
	}
}
