using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour {
	
	private int scoreTextIncrementer;
	public Text scoreText;
	
	public GameObject scoreFloater;
	public GameObject onScreenButtons;
	
	public GameObject gameOverPanel;
	public Text gameOverPanelText;

	public Text[] tutorialText;
	public GameObject[] tutorialHelperObjects;

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
	
	public void Restart()
	{
		TutorialManager.instance.Restart();
		gameOverPanel.SetActive (false);
		scoreText.text = "0";
	}
	
	public void Pause()
	{
		Time.timeScale = 0.0f;
	}
	
	public void UnPause()
	{
		Time.timeScale = 1.0f;
	}
	
	public void MainMenu()
	{
		UnPause();
		Application.LoadLevel ("MainMenu");
	}
	
	public void GameOver()
	{
		gameOverPanel.SetActive (true);
		gameOverPanel.GetComponent<Animator>().SetTrigger("Up");
		gameOverPanelText.text = "Score:\n" + TutorialManager.instance.score;
	}
	
	public void CreateScoreFloater(Vector3 worldPos, int points)
	{
		/*RectTransform canvasTransform = GetComponent<RectTransform>();
		RectTransform rTrans = scoreFloater.GetComponent<RectTransform>();

		Vector2 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
		Vector2 screenPos = new Vector2(
			((viewportPos.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
			((viewportPos.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));

		rTrans.anchoredPosition = screenPos;*/
		
		scoreFloater.GetComponent<Text>().text = "+" + points;
		scoreFloater.gameObject.SetActive (true);
		scoreFloater.GetComponent<ScoreFloater>().ResetFade();
	}

	public void DisplayTutorial(int index)
	{
		foreach (GameObject o in tutorialHelperObjects)
			o.SetActive(false);
		foreach (Text t in tutorialText)
			t.gameObject.SetActive(false);

		tutorialText[index].gameObject.SetActive(true);
		tutorialHelperObjects[index].SetActive(true);
	}
}
