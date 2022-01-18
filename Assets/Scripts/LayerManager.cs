using UnityEngine;

public static class LayerManager {
	
	public static readonly LayerMask GroundLayer = LayerMask.NameToLayer("Ground");
	public static readonly LayerMask CharacterLayer = LayerMask.NameToLayer("Character");
	public static readonly LayerMask WeaponLayer = LayerMask.NameToLayer("Weapon");
}

public static class TagManager {

	public const string MAIN_CAMERA = "MainCamera";
}