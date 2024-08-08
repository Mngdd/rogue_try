using Godot;
using System;

// оно автоматом сделало partial class, так что мб 
// будет хорошо если тут будет лежать только мувмент игрока??
public partial class Player : CharacterBody2D {
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private bool _debugEnabled = false;
	private float _hpSizeX = 0;
	[Export] public int MaxHealth = 100;
	[Export] public int CurrentHealth = 100;

	private bool _dead = false;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


	public override void _Ready() {
		_hpSizeX = GetNode<CanvasLayer>("HUD").GetNode<TextureRect>("HealthBar").Size.X;
	}

	// обработка добавления хп
	public void AddHealth(int amount) {
		int newHealth = amount + CurrentHealth;
		GD.Print("amount: " + amount + ", new: " + newHealth);

		if (newHealth > MaxHealth) {
			CurrentHealth = MaxHealth;
		} else if (newHealth <= 0) {
			CurrentHealth = 0;
			_dead = true;
		} else {
			CurrentHealth = newHealth;
		}
	}

	// _PhysicsProcess в отличие от _Process гарантирует стабильное обновление
	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// user-controlled movement inside (if alive!)
		// Handle Jump.
		if (Input.IsActionJustPressed("move_up") && IsOnFloor() && !_dead)
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// https://docs.godotengine.org/en/4.2/classes/class_input.html#class-input-method-get-vector
		Vector2 direction = Input.GetVector(
			"move_left", "move_right", "move_up", "move_down"
		);
		if (direction != Vector2.Zero && !_dead) {
			velocity.X = direction.X * Speed;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide(); // player update

		if (Position.Y > 10000) // OUT OF BOUNDS, KILL
		{
			AddHealth(-MaxHealth);
			// maybe freeze body when y >10k && _dead
		}
	}

	public override void _Process(double delta) {
		var hud = GetNode<CanvasLayer>("HUD");
		string aliveOrNot = _dead ? "DEAD" : "ALIVE";
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