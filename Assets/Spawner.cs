using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[HideInInspector] public Tile tile;
	public Enemy enemyPrefab;
	public float spawnPeriod;
	float timeSinceLastSpawn;

	void Start() {
		tile = transform.parent.gameObject.GetComponent<Tile>();
	}

    // Update is called once per frame
    void Update()
    {
		timeSinceLastSpawn += Time.deltaTime;
		if (timeSinceLastSpawn > spawnPeriod) {
			Enemy enemy = Instantiate(enemyPrefab);
			enemy.prevTile = tile;
			enemy.transform.localPosition = new Vector3(transform.position.x, 0, transform.position.z);
			timeSinceLastSpawn = 0;
		}
    }
}
