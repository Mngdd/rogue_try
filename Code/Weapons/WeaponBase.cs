using Godot;
using System;

// короче тут такой говнокод будет:
// weapon - шаблон для оружия
// его наследники: gun и melee
// у gun и melee будут свои наследники, чисто уже по типу оружия (пп, пистолет, топор, вилы и т.п.)
public partial class WeaponBase : Node {
	[Export] public string WeaponName = "???";

	[Export(PropertyHint.Enum, "Default,Uncommon,Rare,Legendary")]
	public RarityTypes Rarity = RarityTypes.Default;

	[Export(PropertyHint.Enum, "Default,Toxic,Burn,Freeze")]
	public DamageTypes DamageType = DamageTypes.Default;

	[Export] public float DamageAmount = 0f;


	// TODO: add sound variables
	// TODO: add upgrade slots...
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

	public virtual void Fire() {
	}
}