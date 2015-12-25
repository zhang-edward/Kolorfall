using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameUI gui;

	TetrisGameManager GM_tetris;
	ColorMatchGameManager GM_colorMatch;

	public int score;

	// DEBUG
	public bool DEBUG_MODE = false;
	// DEBUG

	public CameraShake camShake;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		GM_tetris = GetComponentInChildren<TetrisGameManager>();
		GM_colorMatch = GetComponentInChildren<ColorMatchGameManager>();

		GM_tetris.InitBoard();
		GM_colorMatch.InitGrid();

		GetComponent<Animator>().SetTrigger("In");
	}

	public void sendTetrisData(int[,] tetrisPieceData, int tileColor)
	{
		GM_tetris.InitPiece (tetrisPieceData, tileColor);
	}

	public bool haveTetrisPiece()
	{return GM_tetris.HavePiece;}

	public void GameOver()
	{

	}

	public void Restart()
	{
		score = 0;
		GM_colorMatch.ResetGrid();
		GM_tetris.ResetBoard();
	}

	public void CreateScoreFloater(Vector3 pos, int points)
	{
		gui.CreateScoreFloater(pos, points);
	}

	public void CameraShake(float time, float amt)
	{
		camShake.Shake (time, amt);
	}
}
