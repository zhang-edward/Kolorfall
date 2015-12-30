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

	}
}
