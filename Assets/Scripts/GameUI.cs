using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	private int scoreTextIncrementer;
	public Text scoreText;

	void Update()
	{
		if (scoreTextIncrementer < GameManager.instance.score)
			scoreTextIncrementer ++;
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
}
