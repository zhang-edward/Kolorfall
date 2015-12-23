using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	private int scoreTextIncrementer;
	public Text scoreText;

	public GameObject scoreFloater;

	void Update()
	{
		if (scoreTextIncrementer < GameManager.instance.score)
			scoreTextIncrementer ++;
		else if (scoreTextIncrementer > GameManager.instance.score)
			scoreTextIncrementer --;
		scoreText.text = scoreTextIncrementer.ToString ();
	}

	public void Restart()
	{
		GameManager.instance.Restart();
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
		Application.LoadLevel ("MainMenu");
	}

	public void CreateScoreFloater(Vector3 worldPos, int points)
	{
		RectTransform canvasTransform = GetComponent<RectTransform>();
		RectTransform rTrans = scoreFloater.GetComponent<RectTransform>();

		Vector2 viewportPos = Camera.main.WorldToViewportPoint(worldPos);
		Vector2 screenPos = new Vector2(
			((viewportPos.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
			((viewportPos.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f)));

		rTrans.anchoredPosition = screenPos;

		scoreFloater.GetComponent<Text>().text = "+" + points;
		scoreFloater.gameObject.SetActive (true);
	}
}
