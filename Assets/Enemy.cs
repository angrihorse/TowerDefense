using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Paths paths;
	public Tile prevTile;
	Tile nextTile;
	bool moving;

	[Range(0.1f, 3)]
	public float tilesPerSecond;
	float tilePercent;

	void OnEnable() {
		Board.onPathUpdate += TryMoving;
	}

	void OnDisable() {
		Board.onPathUpdate -= TryMoving;
	}

	void Start() {
		TryMoving();
	}

	void TryMoving() {
		if (paths.tileParent.ContainsKey(prevTile) && !moving) {
			nextTile = paths.tileParent[prevTile];
			StartCoroutine(MoveOneTile());
		}
	}

	IEnumerator MoveOneTile() {
		moving = true;
		while (tilePercent < 1f) {
			tilePercent += Time.deltaTime * tilesPerSecond;
			transform.localPosition = Vector3.Lerp(prevTile.transform.position, nextTile.transform.position, tilePercent);
			yield return null;
		}
		moving = false;

		if (paths.destinations.Contains(nextTile)) {
			Destroy(gameObject);
		}

		tilePercent = 0;
		prevTile = nextTile;

		TryMoving();
	}
}
