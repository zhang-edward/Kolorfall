using UnityEngine;
using System.Collections;

public class TetrisTile : GameTile {

	// whether this piece is falling or solid
	public bool isSolid = true;

	void Update()
	{
		UpdateColor();
		if (TileColor == -1)
			isSolid = true;
	}
}
