using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {

	[Header("Setup")]
	public Tilemap tilemap;
	public bool disableTilemapAtRuntime = true;
	public bool fillFloor;
	public bool fillSurrondingFloor;
	public EditorTile defaultFloorTile;
	public Transform staticRoot;
	public Transform dynamicRoot;
	public bool buildNavMesh = true;
	public NavMeshSurface[] navMeshSurfaces;

	[Header("Runtime")]
	[SerializeField]
	private int _width;
	[SerializeField]
	private int _length;

	private void Awake() => Generate();

	public void Generate() {
		Vector3Int size = tilemap.size;
		_width = size.x;
		_length = size.y;

		Vector3Int origin = tilemap.origin;
		Vector3 cellOffset = Vector3.Scale(tilemap.cellSize, tilemap.tileAnchor);
		cellOffset.z = cellOffset.y;
		cellOffset.y = 0f;

		for (int i = 0; i < _width; i++) {
			for (int j = 0; j < _length; j++) {
				Vector3Int pos = new Vector3Int(i, j, 0) + origin;

				EditorTile tile = null;
				bool hasTile = false;
				bool hasFloorBlock = false;
				
				if (tilemap.HasTile(pos)) {
					hasTile = true;
					tile = tilemap.GetTile<EditorTile>(pos);
					if (tile.block) {
						GameObject block = Instantiate(tile.block, tile.canBeStaticBatched ? staticRoot : dynamicRoot);
						Vector3 worldPos = tilemap.CellToWorld(pos);
						block.transform.position = worldPos + cellOffset;
						if (tile.rotatesWithTile) block.transform.localEulerAngles = new Vector3(0f, -tilemap.GetTransformMatrix(pos).rotation.eulerAngles.z, 0f);
						block.transform.position += block.transform.TransformVector(tile.offset);
					}

					hasFloorBlock = tile.floorBlock;
				}

				EditorTile floorTile = fillFloor ? (fillSurrondingFloor || hasTile ? defaultFloorTile : null) : null;
				floorTile = hasFloorBlock ? tile : floorTile;
				
				if (floorTile && floorTile.floorBlock) {
					GameObject floorBlock = Instantiate(floorTile.floorBlock, floorTile.canFloorBeStaticBatched ? staticRoot : dynamicRoot);
					Vector3 worldPos = tilemap.CellToWorld(pos);
					floorBlock.transform.position = worldPos + cellOffset;
					if (floorTile.floorRotatesWithTile) floorBlock.transform.localEulerAngles = new Vector3(0f, -tilemap.GetTransformMatrix(pos).rotation.eulerAngles.z, 0f);
					floorBlock.transform.position += floorBlock.transform.TransformVector(floorTile.floorOffset);
				}
			}
		}
		
		// StaticBatchingUtility.Combine(staticRoot.gameObject);
		
		if (disableTilemapAtRuntime) tilemap.gameObject.SetActive(false);

		if (buildNavMesh && navMeshSurfaces != null) {
			foreach (var surface in navMeshSurfaces) surface.BuildNavMesh();
		}
	}
}