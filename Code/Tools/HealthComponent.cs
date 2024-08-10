using Godot;
using System;

public partial class HealthComponent : Node {
	private bool _dead = false;

	public bool Dead {
		get => _dead;
	}

	[Export] public int MaxHealth = 100;
	[Export] public int CurrentHealth = 100;


	// НЕ ИСПОЛЬЗОВАТЬ ДЛЯ УРОНА, ТК В БУДУЩЕМ ВОЗМОЖНО БУДЕТ РЕЗИСТ УРОНУ
	public void AddHealth(int amount) {
		int newHealth = amount + CurrentHealth;

		if (newHealth > MaxHealth) {
			CurrentHealth = MaxHealth;
		} else if (newHealth <= 0) {
			CurrentHealth = 0;
			_dead = true;
		} else {
			CurrentHealth = newHealth;
		}
	}

	// возможно в будущем будет резист к дамагу, через
	// AddHealth будет неудобно, тк она и для отхила тоже юзается
	public void Damage(int amount) {
		AddHealth(-amount);
	}

	public void Kill() {
		GD.Print("KIIIIIIIIILL");
		CurrentHealth = 0;
		_dead = true;
	}

	public override void _Ready() {
	}

	public override void _Process(double delta) {
	}
}