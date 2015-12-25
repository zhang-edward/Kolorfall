using UnityEngine;
using System.Collections;

public class MainMenuUI : MonoBehaviour {



	public void Play()
	{
		Application.LoadLevel("Game");
	}
	public void WriteReview()
	{
		// TODO: put in url for game
		Application.OpenURL("google.com");
	}
}
