using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[HideInInspector] public Tile tile;
	public Enemy enemyPrefab;
	public Vector2 spawnPeriodRange;

	void Start() {
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn() {
		yield return new WaitForSeconds(UnityEngine.Random.Range(spawnPeriodRange.x, spawnPeriodRange.y));
		Enemy enemy = Instantiate(enemyPrefab);
		enemy.prevTile = tile;
		enemy.transform.localPosition = new Vector3(transform.position.x, 0, transform.position.z);
		StartCoroutine(Spawn());
	}
}
