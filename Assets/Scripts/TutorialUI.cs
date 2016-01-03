using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialUI : GameUI {
	
	private int scoreTextIncrementer;

	public GameObject[] tutorialText;

	void Start()
	{
		SettingsManager.instance.onScreenButtons = onScreenButtons;
	}
	
	void Update()
	{
		if (scoreTextIncrementer < TutorialManager.instance.score)
			scoreTextIncrementer ++;
		else if (scoreTextIncrementer > TutorialManager.instance.score)
			scoreTextIncrementer --;
		scoreText.text = scoreTextIncrementer.ToString ();
	}
	
	public override void Restart()
	{
		TutorialManager.instance.Restart();
		DisplayTutorial(-1);
		scoreText.text = "0";
	}
	
	public override void GameOver()
	{
		DisplayTutorial (7);
	}

	public void DisplayTutorial(int index)
	{
		foreach (GameObject o in tutorialText)
			o.SetActive(false);

		if (index >= 0)
		{
			tutorialText[index].gameObject.SetActive(true);
		}
	}
}
