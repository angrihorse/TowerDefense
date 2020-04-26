using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int position;
	public bool walkable {
		get => contentType == null || contentType.walkable;
	}

	TileContentType contentType;
	GameObject contentInstance;

	public ContentTypes contentTypes;

	public TileContentType Content {
		get => contentType;
		set {
			if (value != null) {
				Set(value);
			} else {
				Clear();
			}
		}
	}

	public void Set(TileContentType content) {
		if (contentInstance != null) {
			Clear();
		}

		contentType = content;
		contentInstance = Instantiate(content.prefab, transform.position, Quaternion.identity);
		contentInstance.transform.localPosition += contentInstance.transform.localScale.y/2 * Vector3.up;

		if (contentType == contentTypes.spawner) {
			contentInstance.GetComponent<Spawner>().tile = this;
		}
	}

	public void Clear() {
		Destroy(contentInstance);
		contentInstance = null;
		contentType = null;
	}
}
