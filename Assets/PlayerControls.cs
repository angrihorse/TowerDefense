using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	public Board board = default;
	public ContentTypes contentTypes;

	Camera camera;
	TileContentType selectedTileContent;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
		selectedTileContent = contentTypes.wall;
    }

	Ray TouchRay => camera.ScreenPointToRay(Input.mousePosition);

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton(0)) {
			board.ClearTile(TouchRay);
		}

		if (Input.GetMouseButton(1)) {
			board.SetTile(TouchRay, selectedTileContent);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			selectedTileContent = contentTypes.wall;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			selectedTileContent = contentTypes.destination;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			selectedTileContent = contentTypes.spawner;
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			selectedTileContent = contentTypes.tower;
		}
    }
}
