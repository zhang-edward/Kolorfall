using UnityEngine;
using System.Collections;

public class TetrisGameManager : MonoBehaviour {

	public static int BOARD_HEIGHT = 10;
	public static int BOARD_WIDTH = 6;
	
	private bool havePiece;
	public bool HavePiece {
		get {return havePiece;}
	}
	// piece that the game is currently placing
	private int[,] curPiece;
	// piece information
	private int pieceWidth;
	private int pieceHeight;

	public float fallInterval = 0.5f;

	private TetrisTile[,] board = new TetrisTile[BOARD_HEIGHT, BOARD_WIDTH];
	// the tetrisTile prefab
	public GameObject tetrisTile;

	public int Combo { get; private set; }

	public Transform BG;

	void Update()
	{
#if UNITY_STANDALONE
		if (GameManager.instance.DEBUG_MODE)
		{
			if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				// if piece is not falling anymore
				if (!FallPiece())
				{
					// set piece to be solid
					for (int y = 0; y < BOARD_HEIGHT; y ++)
					{
						for (int x = 0; x < BOARD_WIDTH; x ++)
						{
							if (!board[y, x].isSolid)
								board[y, x].isSolid = true;
						}
					}
					havePiece = false;
					// check if a row is clear
					CheckClearRow();
				}
			}
		}
		if (havePiece)
		{
			if (Input.GetKeyDown (KeyCode.DownArrow))
				QuickFall();
			if (Input.GetKeyDown (KeyCode.RightArrow))
				moveRight();
			if (Input.GetKeyDown (KeyCode.LeftArrow))
				moveLeft();
		}
#endif
		if (havePiece)
		{
			//TODO: ghost of piece being placed
		}
	}

	public void InitBoard()
	{
		for (int y = 0; y < BOARD_HEIGHT; y ++)
		{
			for (int x = 0; x < BOARD_WIDTH; x ++)
			{
				GameObject o = Instantiate (tetrisTile, Vector3.zero, Quaternion.identity) as GameObject;
				o.transform.SetParent (this.transform);
				o.transform.localPosition = new Vector3(x, y);
				board[y, x] = o.GetComponent<TetrisTile>();
				board[y, x].TileColor = -1;
			}
		}
	}

	public void ResetBoard()
	{
		havePiece = false;
		foreach (TetrisTile tile in board)
			tile.TileColor = -1;
	}

	public void InitPiece(int[,] tetrisPieceData, int tileColor)
	{
		havePiece = true;
		// trim extra space from tetris piece data
		int minY = int.MaxValue;
		int minX = int.MaxValue;
		int maxY = 0;
		int maxX = 0;
		for (int y = 0; y < ColorMatchGameManager.GRID_HEIGHT; y ++)
		{
			for (int x = 0; x < ColorMatchGameManager.GRID_WIDTH; x ++)
			{
				if (tetrisPieceData[y, x] == 1)
				{
					if (x < minX)
						minX = x;
					if (y < minY)
						minY = y;
					if (x > maxX)
						maxX = x;
					if (y > maxY)
						maxY = y;
				}
			}
		}
/*		Debug.Log ("minx: " + minX + "\nminy: " + minY);
		Debug.Log ("maxx: " + maxX + "\nmaxy: " + maxY);
*/
		// +1 to fix an out-of-bounds error, have yet to work out the math
		pieceWidth = maxX - minX + 1;
		pieceHeight = maxY - minY + 1;

		curPiece = new int[pieceHeight, pieceWidth];
		// initialize the array with -1 (empty space)
		// TODO: put array manipulators in a separate class
		ColorMatchGameManager.setArray(curPiece, -1);
		// trim off excess empty space on tetrisPieceData
		for (int y = 0; y < ColorMatchGameManager.GRID_HEIGHT; y ++)
		{
			for (int x = 0; x < ColorMatchGameManager.GRID_WIDTH; x ++)
			{
				if (tetrisPieceData[y, x] == 1)
				{
/*					Debug.Log ("y: " + y + "\nx: " + x);
					Debug.Log ("y - min: " + (y - minY) + "\nx - min: " + (x - minX));*/
					curPiece[y - minY, x - minX] = tileColor;
				}
			}
		}
		PlacePiece();
	}

	private void PlacePiece()
	{
		int middleOfBoard = BOARD_WIDTH / 2;
		int middleOfPiece = pieceWidth / 2;
		int xOffset = middleOfBoard - middleOfPiece;
		int yOffset = BOARD_HEIGHT - pieceHeight;
//		Debug.Log (pieceWidth + " x " + pieceHeight);

		for (int y = 0; y < pieceHeight; y ++)
		{
			for (int x = 0; x < pieceWidth; x ++)
			{
/*				Debug.Log ("x: " + x + "\ny: " + y);
				Debug.Log ("xOffset: " + xOffset + "\nyOffset: " + yOffset);*/
				if (curPiece[y, x] != -1)
				{
					int boardX = x + xOffset;
					int boardY = y + yOffset;
					// if there is a solid tile already at the coordinates, game over
					if (board[boardY, boardX].TileColor != -1)
						GameManager.instance.GameOver();

					board[boardY, boardX].TileColor = curPiece[y, x];
					board[boardY, boardX].isSolid = false;	// set this to be a falling piece
				}
			}
		}
		/*if (!GameManager.instance.DEBUG_MODE)
			StartCoroutine("FallPieceRegular");*/
	}

	private bool FallPiece()
	{
		bool isFalling = true;

/*		// DEBUG
		bool foundPiece = false;
		// DEBUG*/

		for (int y = 0; y < BOARD_HEIGHT; y ++)
		{
			for (int x = 0; x < BOARD_WIDTH; x ++)
			{
				if (!board[y, x].isSolid)	// tile is a falling piece
				{
/*					// DEBUG
					foundPiece = true;
					// DEBUG*/

					// if piece is at bottom or space below piece is a solid tile, stop moving
					if (y == 0 || checkSolid (x, y - 1))
						isFalling = false;
				}
			}
		}

/*		// DEBUG
		if (foundPiece)
			Debug.Log ("Found Piece to fall");
		Debug.Log ("Falling: " + isFalling);
		// DEBUG*/

		// if all the pieces of tetrisPiece are still able to fall
		if (isFalling)
		{
			for (int y = 0; y < BOARD_HEIGHT; y ++)
			{
				for (int x = 0; x < BOARD_WIDTH; x ++)
				{
					if (!board[y, x].isSolid)
					{
						moveTile (y, x, y - 1, x);
					}
				}
			}
		}
		else
		{
			// set piece to be solid
			for (int y = 0; y < BOARD_HEIGHT; y ++)
			{
				for (int x = 0; x < BOARD_WIDTH; x ++)
				{
					if (!board[y, x].isSolid)
						board[y, x].isSolid = true;
				}
			}
			havePiece = false;
			// check if a row is clear
			CheckClearRow();
		}

		return isFalling;
	}

	public void QuickFall()
	{
		// if do not have piece, do not attempt to make a piece fall
		if (!havePiece)
			return;

		GameManager.instance.CameraShake(0.1f, 0.1f);
		while (FallPiece())
		{}
	}

	/*private IEnumerator FallPieceRegular()
	{
		while(havePiece)
		{
			yield return new WaitForSeconds(fallInterval);
			// if piece is not falling anymore
			if (!FallPiece())
			{
				// set piece to be solid
				for (int y = 0; y < BOARD_HEIGHT; y ++)
				{
					for (int x = 0; x < BOARD_WIDTH; x ++)
					{
						if (!board[y, x].isSolid)
							board[y, x].isSolid = true;
					}
				}
				havePiece = false;
				// check if a row is clear
				CheckClearRow();
			}
		}
		//Debug.Log ("FallPiece: Exit");
	}*/

	public void moveRight()
	{
		bool canMove = true;
		for (int y = 0; y < BOARD_HEIGHT; y ++)
			for (int x = 0; x < BOARD_WIDTH; x ++)
				if (!board[y, x].isSolid && (x == BOARD_WIDTH - 1 || checkSolid(x + 1, y)))	 // if trying to move right but piece is at right edge
					canMove = false;

		if (canMove)
		{
			for (int y = 0; y < BOARD_HEIGHT; y ++)
			{
				for (int x = BOARD_WIDTH - 1; x >= 0; x --)
				{
					if (!board[y, x].isSolid)
					{
						moveTile (y, x, y, x + 1);
					}
				}
			}
		}
	}

	public void moveLeft()
	{
		bool canMove = true;
		for (int y = 0; y < BOARD_HEIGHT; y ++)
			for (int x = 0; x < BOARD_WIDTH; x ++)
				if (!board[y, x].isSolid && (x == 0 || checkSolid (x - 1, y)))	// if trying to move right but piece is at right edge
					canMove = false;

		if (canMove)
		{
			for (int y = 0; y < BOARD_HEIGHT; y ++)
			{
				for (int x = 0; x < BOARD_WIDTH; x ++)
				{
					if (!board[y, x].isSolid)
					{
						moveTile (y, x, y, x - 1);
					}
				}
			}
		}
	}

	// ================ NOT BEING USED ================
	public void moveDown()
	{
		FallPiece ();
	}
	// ================ NOT BEING USED ================

	private void CheckClearRow()
	{
		int numRowsCleared = 0;
		// check from top to bottom so cleared rows are not skipped
		for (int y = BOARD_HEIGHT - 1; y >= 0; y --)
		{
			bool rowClear = true;
			for (int x = 0; x < BOARD_WIDTH; x ++)
			{
				// if any empty spaces, row is not clear
				if (board[y, x].TileColor == -1)
					rowClear = false;
			}
			if (rowClear)
			{
				numRowsCleared ++;
				StartCoroutine(clearRow (y));
			}
		}

		if (numRowsCleared > 0)
		{
			Combo ++;

			// POINTS FORMULA
			int pointsScored = (int)(2f + Mathf.Pow (2f, (numRowsCleared))) * Combo;
			GameManager.instance.score += pointsScored;
			GameManager.instance.CreateScoreFloater(Vector3.zero, pointsScored);
		}
		else
		{
			Combo = 0;
		}

		GameManager.instance.CreateScoreMultiplier(Combo);
	}

	private IEnumerator clearRow(int row)
	{
		for (int i = 0; i < BOARD_WIDTH; i ++)
		{
			board[row, i].anim.SetTrigger("Out");
		}

		// wait for animation to finish
		yield return new WaitForSeconds(20f/60f);

		for (int y = row + 1; y < BOARD_HEIGHT; y ++)
		{
			for (int x = 0; x < BOARD_WIDTH; x ++)
			{
				moveTile (y, x, y - 1, x);
			}
		}
	}

	// ====================== Tile Manipulation ====================== //
	private void moveTile(int y, int x, int destY, int destX)
	{
		board[destY, destX].TileColor = board[y, x].TileColor;
		board[destY, destX].isSolid = board[y, x].isSolid;
		board[y, x].TileColor = -1;
	}

	private bool checkSolid(int x, int y)
	{
		return board[y, x].TileColor != -1 && board[y, x].isSolid;
	}
}
