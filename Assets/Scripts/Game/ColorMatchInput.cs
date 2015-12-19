using UnityEngine;
using System.Collections;

public class ColorMatchInput : MonoBehaviour {

	private ColorMatchGameManager cmgm;

	void Start()
	{
		cmgm = GetComponent<ColorMatchGameManager>();
	}

	void Update()
	{
		if (!GameManager.instance.haveTetrisPiece())	// only able to match colors if there is no tetris piece falling
		{
			if (Input.GetMouseButton(0))
			{
				//Debug.Log ("Mouse Down");
				Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				int x = Mathf.RoundToInt (screenPos.x - transform.position.x);
				int y = Mathf.RoundToInt (screenPos.y - transform.position.y)/* *-1 */;		// times -1 because the tiles are positioned that way

				// check if x and y are in bounds
				if (x < ColorMatchGameManager.GRID_WIDTH && 
				    y < ColorMatchGameManager.GRID_HEIGHT &&
				    x >= 0 && y >= 0)
				{
					// grid is not empty at that position
					if (cmgm.getTileColor(x, y) != -1)
						cmgm.MatchTiles (x, y);
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				//Debug.Log ("Mouse Up");
				if (cmgm.ClearedList.Count > 0)
				{
					cmgm.ClearTiles ();
					cmgm.ClearedList.Clear();
					cmgm.CheckAlert ();
				}
			}
		}
	}
}
