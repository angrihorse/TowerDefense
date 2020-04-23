using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{
	public Transform arrowPrefab;
	public Paths paths;

	List<Transform> arrows = new List<Transform>();

    void OnEnable() {
		UpdateArrows();
		Board.onPathUpdate += UpdateArrows;
	}

	void OnDisable() {
		Clear();
		Board.onPathUpdate -= UpdateArrows;
	}

	void Clear() {
		if (arrows != null) {
			foreach (Transform arrow in arrows) {
				Destroy(arrow.gameObject);
			}

			arrows.Clear();
		}
	}

    void UpdateArrows()
    {
		Clear();
		foreach(Tile tile in paths.tileParent.Keys) {
			Tile parentTile = paths.tileParent[tile];
			if (parentTile == null || paths.destinations.Contains(tile)) {
				continue;
			}

			Transform arrow = Instantiate(arrowPrefab, tile.transform, true);
			arrow.localPosition = Vector3.back * 0.001f;

			Vector2Int dir = parentTile.position - tile.position;
			arrow.localRotation = directionToArrowRotation[dir];

			arrows.Add(arrow);
		}
    }

	public static Dictionary<Vector2Int, Quaternion> directionToArrowRotation = new Dictionary<Vector2Int, Quaternion>() {
		{new Vector2Int(0, 1), Quaternion.Euler(0f, 0f, 0f)},
		{new Vector2Int(-1, 0), Quaternion.Euler(0f, 0f, 90f)},
		{new Vector2Int(0, -1), Quaternion.Euler(0f, 0f, 180f)},
		{new Vector2Int(1, 0), Quaternion.Euler(0f, 0f, 270f)},
	};
}
