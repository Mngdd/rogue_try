using Godot;
using System;
using System.Collections.Generic;
using rogue_try.Code.Weapons;

public partial class CombatComponent : Node {
	[Export] public int SlotsAmount = 1;
	
	// это дорого, но того стоит, у нас разные классы оружия
	// а ебаться с кастованием их от weaponbase до их класса я не хочу
	public Godot.Collections.Array WeaponSlots = new Godot.Collections.Array();

	private int _currentWeaponInd = 0;

	[Export]
	private int CurrentWeaponInd {
		get => _currentWeaponInd;
		set {
			if (0 <= value && value < SlotsAmount)
				_currentWeaponInd = value;
		}
	}


	public override void _Ready() {
	}

	public void SwapNext() {
		_currentWeaponInd = (_currentWeaponInd + 1) % SlotsAmount;
	}

	public void SwapPrev() {
		if (_currentWeaponInd == 0) {
			_currentWeaponInd = SlotsAmount - 1;
			return;
		}

		_currentWeaponInd = SlotsAmount - 1;
	}


	// public weaponType DropByIndex<weaponType>(int index) {
	// 	// мы получили не то оружие, пепец...
	// 	if (!(WeaponSlots[index].GetType() is weaponType)) {
	// 		throw new Exception($"DropByIndex: tried to get by {index} index, " +
	// 		                    $"got WRONG type: {WeaponSlots[index].GetType()}");
	// 	}
	//
	// 	// return WeaponSlots[index].As<weaponType>();
	// }
}