using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public static List<Vector2Int> directions = new List<Vector2Int>() {
		new Vector2Int(0, 1), new Vector2Int(0, -1),
		new Vector2Int(1, 0), new Vector2Int(-1, 0),
	};

	public Vector2Int size;
	public Tile tilePrefab;
	public Tile[,] tiles;

	public TileContentType destination;
	public Paths paths;

	public delegate void BoardStateChangeEvent();
	public static event BoardStateChangeEvent onPathUpdate;

    // Start is called before the first frame update
    void Start()
    {
		tiles = new Tile[size.x, size.y];
		Vector3 offset = new Vector3((size.x - 1) * 0.5f, 0f, (size.y - 1) * 0.5f);
		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				Tile tile = Instantiate(tilePrefab, transform);
				tile.position = new Vector2Int(x, y);
				tile.transform.localPosition = new Vector3(x, 0f, y) - offset;
				tiles[x, y] = tile;
			}
		}
    }

	public void ClearTile(Ray touchRay) {
		Tile tile = TileFromRay(touchRay);
		if (tile == null || tile.Content == null) {
			return;
		}

		if (tile.Content == destination) {
			paths.destinations.Remove(tile);
		}

		if (!tile.walkable || tile.Content == destination) {
			tile.Content = null;
			BreadthFirstSearch();
			return;
		}

		tile.Content = null;
	}

	public void SetTile(Ray touchRay, TileContentType content) {
		Tile tile = TileFromRay(touchRay);
		if (tile == null || tile.Content != null) {
			return;
		}

		tile.Set(content);
		if (content == destination) {
			paths.destinations.Add(tile);
		}

		if (!tile.walkable || tile.Content == destination) {
			BreadthFirstSearch();
		}
	}

	Tile TileFromRay(Ray ray) {
		if (Physics.Raycast(ray, out RaycastHit hit)) {
			int x = (int)(hit.point.x + size.x * 0.5f);
			int y = (int)(hit.point.z + size.y * 0.5f);
			if (WithinBoardLimits(x, y)) return tiles[x, y];
		}

		return null;
	}

	void BreadthFirstSearch() {
		Queue<Tile> frontier = new Queue<Tile>();
		paths.tileParent.Clear();

		foreach (Tile tile in paths.destinations) {
			frontier.Enqueue(tile);
		}

		while (frontier.Count > 0) {
			Tile currentTile = frontier.Dequeue();
			foreach (Tile neighbor in GetNeighbors(currentTile)) {
				if (!paths.tileParent.ContainsKey(neighbor)) {
					frontier.Enqueue(neighbor);
					paths.tileParent[neighbor] = currentTile;
				}
			}
		}

		if (onPathUpdate != null) {
			onPathUpdate();
		}
	}

	List<Tile> GetNeighbors(Tile tile) {
		List<Tile> neighbors = new List<Tile>();
		foreach (Vector2Int dir in directions) {
			Vector2Int pos = tile.position + dir;
			if (WithinBoardLimits(pos.x, pos.y) && tiles[pos.x, pos.y].walkable) {
				neighbors.Add(tiles[pos.x, pos.y]);
			}
		}

		return neighbors;
	}

	public bool WithinBoardLimits(int x, int y) {
		return x >= 0 && x < size.x && y >= 0 && y < size.y;
	}
}
