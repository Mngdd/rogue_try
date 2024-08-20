using Godot;
using System;
using BulletTargets = Bullet.BulletTargets;

// короче тут такой говнокод будет:
// weapon - шаблон для оружия
// его наследники: gun и melee
// у gun и melee будут свои наследники, чисто уже по типу оружия (пп, пистолет, топор, вилы и т.п.)
public partial class WeaponBase : Node2D {
	[Export] public string WeaponName = "UNDEFINED";
	[Export] public double CooldownTime = 1d;
	[Export] public int DamageBase = 0;

	[Export(PropertyHint.Enum, "Default,Uncommon,Rare,Legendary")]
	public RarityTypes Rarity = RarityTypes.Default;
	
	public BulletTargets Target = BulletTargets.None;

	[Export(PropertyHint.Enum, "Default,Toxic,Burn,Freeze")]
	public DamageTypes DamageType = DamageTypes.Default;

	public bool IsMelee; // >:) костылииии
	[Export] public float DamageAmount = 0f;

	private protected Timer CooldownTimer = new Timer();
	private protected bool CanShootCooldown = true;

	// TODO: add sound variables
	// TODO: add upgrade slots...
	// TODO: REMOVE OBSOLETE ENUMS
	// не забудь поменять и [export enum] строчку если чета меняешь в enum!!
	public enum DamageTypes {
		Default,
		Toxic,
		Burn,
		Freeze
	}

	public enum RarityTypes {
		Default,
		Uncommon,
		Rare,
		Legendary
	}

	// ОЧЕНЬ ВАЖНО, КАЖДЫЙ НОВЫЙ ТИП ОРУЖИЯ ДОЛЖЕН ЗДЕСЬ БЫТЬ
	// Т.К. Я БУДУ КАСТОВАТЬ ИЗ WeaponBase В НУЖНЫЙ КЛАСС ПО ЭТОМУ Enum
	public enum WeaponClass {
		Pistol,
		SubMachine,
		AutoRifle,
		BurstRifle,
		Sniper,
		RocketLauncher,
		GrenadeLauncher,
		Melee // TODO мили оружие я нихуя не придумал, пока затычка
	}

	public override void _Ready() {
		AddChild(CooldownTimer);
		CooldownTimer.WaitTime = CooldownTime;
		CooldownTimer.OneShot = true;
		Callable onCooldownEnd = Callable.From(() => CanShootCooldown = true);
		CooldownTimer.Connect("timeout", onCooldownEnd);
	}

	public virtual void Fire() {
		if (Target == BulletTargets.None) //todo: delete this when finish combat system!!!
			throw new ArgumentException("GOT TARGET TYPE NONE");
	}

	public virtual void Reaload() {
	}

	private protected virtual bool _canShoot() {
		return CanShootCooldown;
	}
}