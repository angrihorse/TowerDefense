using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Paths : ScriptableObject
{
	public HashSet<Tile> destinations = new HashSet<Tile>();
    public Dictionary<Tile, Tile> tileParent = new Dictionary<Tile, Tile>();	
}
