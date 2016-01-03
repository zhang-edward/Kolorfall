using UnityEngine;
using System.Collections;

public class TutorialManager : GameManager {
	
	public TutorialUI tutorialGui;
	
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

		StartCoroutine ("Tutorial");
	}

	IEnumerator GameOverScreen()
	{
		yield return new WaitForSeconds(1.0f);
		tutorialGui.GameOver();
	}

	public override void CreateScoreFloater(Vector3 pos, int points)
	{
		tutorialGui.CreateScoreFloater(pos, points);
	}

	public override void CreateScoreMultiplier(int multiplier)
	{
		tutorialGui.SetScoreMultiplier(multiplier);
	}
	
	public void DisplayTutorial(int index)
	{
		tutorialGui.DisplayTutorial(index);
	}

	IEnumerator Tutorial()
	{
		DisplayTutorial(0);
		while(!GM_tetris.HavePiece)
			yield return null;

		DisplayTutorial (1);
		yield return new WaitForSeconds(5.0f);
		DisplayTutorial (2);
		while (GM_tetris.HavePiece)
			yield return null;

		DisplayTutorial(3);
		while (GM_tetris.Combo == 0)
			yield return null;

		DisplayTutorial(4);
		yield return new WaitForSeconds(5.0f);

		DisplayTutorial (5);
		yield return new WaitForSeconds(5.0f);

		DisplayTutorial(6);
	}
}
