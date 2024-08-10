using Godot;
using System;

// оно автоматом сделало partial class, так что мб 
// будет хорошо если тут будет лежать только мувмент игрока??
public partial class Player : CharacterBody2D {
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public Node HpSystem;
	public int MaxHealth {
		get => (int)HpSystem.Get("MaxHealth");
	}
	public int CurrentHealth {
		get => (int)HpSystem.Get("CurrentHealth");
	}
	private bool _debugEnabled = false;
	private float _hpSizeX = 0;
	
	[Export] PackedScene bullet_scn; // TODO: DELME

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


	public override void _Ready() {
		HpSystem = GetNode<Node>("HealthComponent");
		_hpSizeX = GetNode<CanvasLayer>("HUD").GetNode<TextureRect>("HealthBar").Size.X;
		}

	// _PhysicsProcess в отличие от _Process гарантирует стабильное обновление
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		if (Input.IsActionPressed("key_shoot")) {
			RigidBody2D bullet = bullet_scn.Instantiate<RigidBody2D>();
			
			bullet.Rotation = GetAngleTo(GetGlobalMousePosition());
			bullet.GlobalPosition = GlobalPosition;
			bullet.LinearVelocity = bullet.Transform.X * 200;
			
			GetTree().Root.AddChild(bullet);
		}
		
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("move_up") && IsOnFloor() && !(bool)HpSystem.Get("Dead"))
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector(
			"move_left", "move_right", "move_up", "move_down"
		);
		if (direction != Vector2.Zero && !(bool)HpSystem.Get("Dead")) {
			velocity.X = direction.X * Speed;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		Velocity = velocity;
		MoveAndSlide(); // player update
		
		if (Position.Y > 10000) // OUT OF BOUNDS, KILL
		{
			HpSystem.Call("Kill");
			// maybe freeze body when y >10k && _dead
		}
	}

	public override void _Process(double delta) {
		var hud = GetNode<CanvasLayer>("HUD");

		string aliveOrNot = (bool)HpSystem.Get("Dead") ? "DEAD" : "ALIVE";
		hud.GetNode<Label>("debug").Text =
			@$"DEBUG INFO:
			POS X: {Position.X}
			POS Y: {Position.Y}
			VEL X: {Velocity.X}
			VEL Y: {Velocity.Y}
			----
			STATUS: {aliveOrNot}
			HP: {CurrentHealth}";

		var hpBar = hud.GetNode<TextureRect>("HealthBar");
		hpBar.Size = new Vector2(_hpSizeX * ((float)CurrentHealth / MaxHealth), hpBar.Size.Y);
	}
}