using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorMatchGameManager : MonoBehaviour {

	public static int GRID_HEIGHT = 8;
	public static int GRID_WIDTH = 5;
	public static int TURNS_PER_PROG = 2;

	private int turns;	// 1 turn is 1 match
	private int numColors = 4;

	private GameTile[,] grid = new GameTile[GRID_HEIGHT, GRID_WIDTH];
	public GameObject tile;

	// number of connected tiles with same color
	private int testTileColor;
	private List<GameTile> clearedList = new List<GameTile>();
	public List<GameTile> ClearedList{
		get{return clearedList;}
	}
	private int[,] testGrid = new int[GRID_HEIGHT, GRID_WIDTH];
	private int[,] tetrisPieceData = new int[GRID_HEIGHT, GRID_WIDTH];

	public Transform BG;
	
	void Update()
	{
		if (GameManager.instance.DEBUG_MODE)
		{
			// DEBUG
			if (Input.GetKeyDown(KeyCode.Space))
			{
				//Debug.Log ("Gen Row");
				StartCoroutine("GenerateRow");
			}
		}

		// "growing" effect of selected tiles
		foreach (GameTile gTile in grid)
		{
			if (clearedList.Contains (gTile))
			{
				gTile.transform.localScale = new Vector3 (1.3f, 1.3f);
				gTile.sr.sortingOrder = 1;
			}
			else
			{
				gTile.transform.localScale = Vector3.one;
				gTile.sr.sortingOrder = 0;
			}
		}
	}

	public void InitGrid()
	{
		// create tiles
		for (int y = 0; y < GRID_HEIGHT; y ++)
		{
			for (int x = 0; x < GRID_WIDTH; x ++)
			{
				GameObject o = Instantiate (tile, Vector3.zero, Quaternion.identity) as GameObject;
				o.transform.SetParent (this.transform);
				o.transform.localPosition = new Vector3(x, /*-*/y);

				GameTile gt = o.GetComponent<GameTile>();

				grid[y, x] = gt;
				gt.TileColor = -1;
			}
		}
		GenerateGrid ();
	}

	public void ResetGrid()
	{
		turns = 0;
		foreach(GameTile tile in grid)
		{
			tile.TileColor = -1;
		}
		GenerateGrid();
	}

	private void GenerateGrid()
	{
		for (int y = 0; y < GRID_HEIGHT / 3; y ++)	// board generates only 1/3 of the screen
		{
			for (int x = 0; x < GRID_WIDTH; x ++)
			{
				// generate a random tile, with param=numColors number of different tiles
				grid[y, x].TileColor = Random.Range (0, numColors);
				grid[y, x].anim.SetTrigger("In");
			}
		}
	}

	// pre: no tiles in last row
	private IEnumerator GenerateRow()
	{
		for (int y = 0; y < GRID_HEIGHT; y ++)
		{
			for (int x = 0; x < GRID_WIDTH; x ++)
			{
				// if there is a tile on the last row, game over
				if (y == GRID_HEIGHT - 2 && grid[y, x].TileColor != -1)
					GameManager.instance.GameOver();
				grid[y, x].anim.SetTrigger("Rise");
			}
		}

		// wait for animation to finish
		yield return new WaitForSeconds(20f/60f);

		for (int y = GRID_HEIGHT - 2; y >= 0; y --)
		{
			for (int x = 0; x < GRID_WIDTH; x ++)
			{
				grid[y + 1, x].TileColor = grid[y, x].TileColor;
			}
		}

		// generate new row
		for (int x = 0; x < GRID_WIDTH; x ++)
		{
			grid[0, x].TileColor = Random.Range (0, numColors);
			grid[0, x].anim.SetTrigger ("In");
		}
	}

	public void CheckAlert()
	{
		bool alert = false;
		for (int x = 0; x < GRID_WIDTH; x ++)
		{
			// if there is a tile on the 2nd to last row, begin alert animation
			if (grid[GRID_HEIGHT - 3, x].TileColor != -1)
				alert = true;
		}
		BG.GetComponent<Animator>().SetBool("Alert", alert);
	}

	// Puts adjacent same-color tiles in the clearedList
	public void MatchTiles(int x, int y)
	{
		// reset variables necessary for processing matches
		testTileColor = -1;
		clearedList.Clear ();
		setArray(tetrisPieceData, -1);

		// copy the grid to a test grid to manipulate as we wish
		CopyGrid(grid, testGrid);

		// start recursion
		testTile (x, y);


	}

	// clear the tiles in the clearedList and make other tiles fall
	// also send the piece data to the tetris board
	public void ClearTiles()
	{
		// after testing, set each cleared tile to be destroyed
		foreach (GameTile cmTile in clearedList)
		{
			// TODO: effects and shit
			cmTile.TileColor = -1;
		}
		GameManager.instance.score += clearedList.Count;
		// TODO: floating message which says how many points earned

		
		// send the piece data to the tetris game
		GameManager.instance.sendTetrisData(tetrisPieceData, testTileColor);

		checkFalling();	// any piece with empty space below it falls down

		turns ++;
		// every TURNS_PER_PROG matches, generate a new row
		// also generate a new row if the board is somehow completely empty (very rare)
		if (turns % TURNS_PER_PROG == 0 || checkIfBoardEmpty())
			StartCoroutine("GenerateRow");
	}

	// recursive method
	private void testTile(int x, int y)
	{
		//Debug.Log ("testing " + x + " and " + y);
		if (testGrid[y, x] == -1)
			return;
		// test a tile
		if (testTileColor == -1)
		{
			// set this to be the color to be tested in the recursion
			testTileColor = testGrid[y, x];
			// mark this tile as tested on testGrid, add this to the list of tested tiles, and set this to be added to a tetris piece
			testGrid[y, x] = -1;
			clearedList.Add (grid[y, x]);
			tetrisPieceData[y, x] = 1;
		}
		// if this tile is not the same as the testTileColor
		else if (testTileColor != testGrid[y, x])
			return;
		// if this tile is the same as the testTileColor
		else
		{
			// mark this tile as tested on testGrid, add this to the list of tested tiles, and set this to be added to a tetris piece
			testGrid[y, x] = -1;
			clearedList.Add (grid[y, x]);
			tetrisPieceData[y, x] = 1;
		}

		// test all surrounding tiles
		if (x > 0)
			testTile (x - 1, y);
		if (y > 0)
			testTile (x, y - 1);
		if (x < GRID_WIDTH - 1)
			testTile (x + 1, y);
		if (y < GRID_HEIGHT - 1)
			testTile (x, y + 1);
	}

	private void checkFalling()
	{
		// if tiles have fallen
		bool tilesFell = true;
		// tiles fall until no more tiles can fall
		while (tilesFell == true)
		{
			tilesFell = false;
			// Debug.Log ("Tiles Falling");
			for (int y = 1; y < GRID_HEIGHT; y ++)	// do not check bottom row, falling tiles are impossible there
			{
				for (int x = 0; x < GRID_WIDTH; x ++)
				{
					// if the grid space below this tile is empty, drop down one tile space
					if (grid[y, x].TileColor != -1 && grid[y - 1, x].TileColor == -1)	// do not check empty space tiles
					{
						grid[y - 1, x].TileColor = grid[y, x].TileColor;
						grid[y, x].TileColor = -1;
						tilesFell = true;
					}
				}
			}
		}
	}

	private bool checkIfBoardEmpty()
	{
		foreach(GameTile tile in grid)
		{
			if (tile.TileColor != -1)
				return false;
		}
		return true;
	}


	//======================= Array manipulation methods and other utility methods =========================//
	private void CopyGrid(GameTile[,] source, int[,] dest)
	{
		for (int y = 0; y < GRID_HEIGHT; y ++)
		{
			for (int x = 0; x < GRID_WIDTH; x ++)
			{
				dest[y, x] = grid[y, x].TileColor;
			}
		}
	}
	
	public static void setArray(int[,] arr, int value)
	{
		for (int r = 0; r < arr.GetLength(0); r ++)
		{
			for (int c = 0; c < arr.GetLength (1); c ++)
			{
				arr[r, c] = value;
			}
		}
	}

	public int getTileColor(int x, int y)
	{
		return grid[y, x].TileColor;
	}
}
