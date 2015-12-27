using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameUI gui;

	TetrisGameManager GM_tetris;
	ColorMatchGameManager GM_colorMatch;

	public ColorMatchInput Input_cm;
	public TetrisTouchInput Input_tetris;

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
		// shake vigorously to show that player has lost
		camShake.Shake (0.1f, 1.0f);

		Input_cm.enabled = false;
		Input_tetris.enabled = false;

		StartCoroutine ("GameOverScreen");
	}

	IEnumerator GameOverScreen()
	{
		yield return new WaitForSeconds(1.0f);
		gui.GameOver();
	}

	public void Restart()
	{
		// reset score, call reset methods for game managers
		score = 0;

		Input_cm.enabled = true;
		Input_tetris.enabled = true;

		GM_colorMatch.ResetGrid();
		GM_tetris.ResetBoard();
	}

	/*public void CreateScoreFloater(Vector3 pos, int points)
	{
		gui.CreateScoreFloater(pos, points);
	}*/

	public void CameraShake(float time, float amt)
	{
		camShake.Shake (time, amt);
	}
}
