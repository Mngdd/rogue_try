using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Godot.Collections;
using BulletTargets = Bullet.BulletTargets;


public partial class CombatComponent : Node2D {
	[Signal]
	public delegate void WeaponChangedEventHandler();

	private Node2D _currentWeap;
	private int _currentWeaponInd = 0;
	private bool _currIsMelee;
	
	// only needed to export weapons from editor to c# WeaponSlots list
	[Export] public Array<PackedScene> WeaponsOnSpawn = new Array<PackedScene>();
	// contains weapon scenes
	public List<PackedScene> WeaponSlots = new List<PackedScene>();
	[Export] public int SlotsAmount;
	[Export] public BulletTargets Target;
	[Export]
	private int CurrentWeaponInd {
		get => _currentWeaponInd;
		set {
			_checkIndex(value);
			_currentWeaponInd = value;
		}
	}


	private void _checkIndex(int index) {
		if (0 > index || index >= SlotsAmount)
			throw new IndexOutOfRangeException();
	}
	
	public override void _Ready() {
		if (SlotsAmount <= 0) {
			throw new ValidationException("Combat component equals a non-positive number, " +
			                              "change it in the editor!");
		}

		_checkIndex(_currentWeaponInd);
		int maxSize = (SlotsAmount > WeaponsOnSpawn.Count) ? WeaponsOnSpawn.Count : SlotsAmount;
		for (int i = 0; i < maxSize; ++i) {
			WeaponSlots.Add(WeaponsOnSpawn[i]);
		}
		EmitSignal(SignalName.WeaponChanged);
	}

	public void SwapNext() {
		_currentWeaponInd = (_currentWeaponInd + 1) % SlotsAmount;
		EmitSignal(SignalName.WeaponChanged);
	}

	public void SwapPrev() {
		if (_currentWeaponInd == 0) {
			_currentWeaponInd = SlotsAmount - 1;
		} else {
			--_currentWeaponInd;
		}

		EmitSignal(SignalName.WeaponChanged);
	}


	public void OnWeaponChanged() { //FIXME!
		GD.Print("WEP CHANGIN TO " + _currentWeaponInd);
		if (SlotsAmount == 0) // no weapons => no changes here
			return;
		_currentWeap = WeaponSlots[_currentWeaponInd].Instantiate<Node2D>();
		_currentWeap.Set("Target", (int)Target);  // fixme, maybe bad idea: (int)Target!!!!!
		AddChild(_currentWeap);
		_currIsMelee = (bool)WeaponSlots[_currentWeaponInd].Get("IsMelee");
	}
	
	public void Shoot() {
		_currentWeap.Call("Fire");
	}

	public void Reload() {
		_currentWeap.Call("Reload");
	}
	// TODO: GET/SET ARE OBSOLETE? LIST IS PUBLIC RN
	// returns WeaponFirearm or WeaponMelee by index
	// public TWeaponType Get<TWeaponType>(int index) {
	// 	
	// 	_checkIndex(index);
	// 	
	// 	// TWeaponType != melee and != firearm
	// 	
	// 	if (!typeof(TWeaponType).IsAssignableFrom(typeof(WeaponFirearm)) &&
	// 	    !typeof(TWeaponType).IsAssignableFrom(typeof(WeaponMelee))) {
	// 		throw new ArgumentException("Drop() got unexpected weapon class: " + typeof(TWeaponType));
	// 	}
	// 	
	// 	//TODO: check if IsMelee ~ weaponType==WeaponMelee, cuz im not sure in typeof but...
	// 	if (_weaponSlots[index].IsMelee != (typeof(TWeaponType) == typeof(WeaponMelee))) {
	// 		GD.Print("wataheeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeel, fixme maybe???");
	// 	}
	// 	
	// 	// RETURN MELEE OR FIREARM
	// 	if (_weaponSlots[index].IsMelee) {
	// 		return (TWeaponType)Convert.ChangeType(
	// 			_weaponSlots[index].WeapMelee, typeof(TWeaponType)
	// 			);
	// 	}
	//
	// 	return (TWeaponType)Convert.ChangeType(
	// 		_weaponSlots[index].WeapFire, typeof(TWeaponType)
	// 	);
	// }

	// public void Remove(int index) {
	// 	_checkIndex(index);
	// 	_weaponSlots[index] = new SlotCell { IsEmpty = true};
	// }
	//
	// // changes slot[index] weapon to another WeaponFirearm or WeaponMelee
	// public void Set<TWeaponType>(int index, TWeaponType pickUpWeapon) {
	// 	_checkIndex(index);
	// 	
	// 	if (!typeof(TWeaponType).IsAssignableFrom(typeof(WeaponFirearm)) &&
	// 	    !typeof(TWeaponType).IsAssignableFrom(typeof(WeaponMelee))) {
	// 		throw new ArgumentException("Set() got unexpected weapon class: " + typeof(TWeaponType));
	// 	}
	// 	
	// 	// all ok, set it now
	// 	bool isMelee = typeof(TWeaponType) == typeof(WeaponMelee);
	// 	
	// 	if (isMelee) {
	// 		_weaponSlots[index] = new SlotCell { IsEmpty = false, IsMelee = isMelee};
	// 		return;
	// 	}
	// 	_weaponSlots[index] = new SlotCell { IsEmpty = false, IsMelee = isMelee};
	// }
	//
	// changes current weapon to another WeaponFirearm or WeaponMelee
	// public void SetCurrent<TWeaponType>(TWeaponType pickUpWeapon) {
	// 	Set(_currentWeaponInd, pickUpWeapon);
	// }
}