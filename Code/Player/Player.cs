using Godot;
using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;

// оно автоматом сделало partial class, так что мб 
// будет хорошо если тут будет лежать только мувмент игрока??
public partial class Player : CharacterBody2D
{
	enum Direction {
		none, left, right, up, down
	};
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public const float GlidingRatio = 0.08f;
	public const float TurningRatio = 0.30f;
	public const int CoyoteDelay = 10;
	public const float WallSlideVelocity = 20.0f;
	public const float WallJumpRatio = 0.8f;

	// flags to handle coyote jumpes
	private bool _CanJump = true;
	private bool _CoyoteTimerStarted = false;
	// timer to handle coyote jumpes
	private int _CoyoteTimer = 0;
	// flags to handle wall jumping
	private bool GluedToWall = false;
	private Direction LastDirection = Direction.none;
	private Direction WallJumpDirection = Direction.none;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	// _PhysicsProcess в отличие от _Process гарантирует стабильное обновление
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector(
			"move_left", "move_right", "move_up", "move_down"
			);

		ProcessVerticalMovement(ref velocity, ref delta, ref direction);
		ProcessHorisontalMovement(ref velocity, ref direction);

		Velocity = velocity;
		MoveAndSlide(); // обновляет велосити как я понял...
	}

	// Processes jump and gravity mechanics
	private void ProcessVerticalMovement (ref Vector2 velocity, ref double delta, ref Vector2 direction) {

		if (IsOnFloor()) {
			_CanJump = true;
			GluedToWall = false;
			_CoyoteTimer = 0;
			_CoyoteTimerStarted = false;
		} else if (_WallJumpChecks()) {
			if ((LastDirection == Direction.left || LastDirection == Direction.right) && (!GluedToWall)) {
				GluedToWall = true;
				_CanJump = true;
				_CoyoteTimerStarted = false;
				_CoyoteTimer = 0;
				_SetJumpDirection();
			}
			velocity.Y = WallSlideVelocity;
		}
		else {
			// Handle coyote timer
			_CoyoteTimerCycle();
			// Add the gravity.
			velocity.Y += gravity * (float)delta;
		}

		// Handle Jump.
		if (_CanJump) {
			_JumpProcess(ref velocity, ref direction);
		}
	}

	// Processes player horisontal movement
	private void ProcessHorisontalMovement(ref Vector2 velocity, ref Vector2 direction) {
		// Get the input direction and handle the movement/deceleration.
		// https://docs.godotengine.org/en/4.2/classes/class_input.html#class-input-method-get-vector
		// Changes velocity if there's horisontal movement
		if ((direction.X != 0) && (!GluedToWall)) {
			_Turn(ref direction, ref velocity);
		}
		// Returns velocity to base value if there's no horisontal input
		else {
			_Glide(ref velocity);
		}
		if (direction.X > 0) {
			LastDirection = Direction.right;
		} else if (direction.X < 0) {
			LastDirection = Direction.left;
		} else {
			LastDirection = Direction.none;
		}
	}

	// Slowly sets velocity to zero if movement keys are not pressed and character is on floor
	private void _Glide(ref Vector2 velocity) {
		if (IsOnFloor()) {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, GlidingRatio*Speed);
		}
	}
	// Slowly sets velocity to the value that depend on direction inputed by the player
	private void _Turn(ref Vector2 direction, ref Vector2 velocity) {
		velocity.X = Mathf.MoveToward(Velocity.X, direction.X*Speed, TurningRatio*Speed);
	}

	// Handle coyote timer
	private void _CoyoteTimerCycle() {
		if (_CoyoteTimer != 0) {
				_CoyoteTimer--;
		} else {
			if (_CanJump && (!_CoyoteTimerStarted)) {
				_CoyoteTimer = CoyoteDelay;
				_CoyoteTimerStarted = true;
			} else if (_CanJump && _CoyoteTimerStarted) {
				_CanJump = false;
			}
		}
	}
	// Processes jump
	private void _JumpProcess(ref Vector2 velocity, ref Vector2 direction) {
		if (!GluedToWall && Input.IsActionJustPressed("move_up")) {
			velocity.Y = JumpVelocity;
			_CanJump = false;
		} else if (GluedToWall) {
			if (WallJumpDirection == Direction.left) {
				if ((direction.Y < 0) && (direction.X < 0)) {
					velocity.Y = WallJumpRatio*JumpVelocity;
					WallJumpDirection = Direction.none;
					GluedToWall = false;
					_CanJump = false;
				}
			} else if (WallJumpDirection == Direction.right) {
				if ((direction.Y < 0) && (direction.X > 0)) {
					velocity.Y = WallJumpRatio*JumpVelocity;
					GluedToWall = false;
					WallJumpDirection = Direction.none;
					_CanJump = false;
				}
			}
		}
	}
	private bool _WallJumpChecks() {
		return GluedToWall || (IsOnWall() &&
		(((LastDirection == Direction.left || LastDirection == Direction.right) && (!GluedToWall)) ||
		 GluedToWall));
	}
	private void _SetJumpDirection() {
		if (WallJumpDirection == Direction.none) {
			if (LastDirection == Direction.left) {
				WallJumpDirection = Direction.right;
			} else if (LastDirection == Direction.right) {
				WallJumpDirection = Direction.left;
			}
		}
	}
}
