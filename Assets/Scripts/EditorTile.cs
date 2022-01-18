using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "Editor Tile", order = 357)]
public sealed class EditorTile : Tile {
	public string identifier;
	public bool canBeStaticBatched;
	public bool canFloorBeStaticBatched = true;
	public bool rotatesWithTile;
	public bool floorRotatesWithTile;
	public Vector3 offset;
	public Vector3 floorOffset;
	public GameObject block;
	public GameObject floorBlock;
}