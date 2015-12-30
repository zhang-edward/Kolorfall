using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreMultiplier : MonoBehaviour {
	
	Text text;
	
	void Awake()
	{
		text = GetComponent<Text>();
	}

	public void SetText (int multiplier)
	{
		text.text = "X" + multiplier;
		text.fontSize = 13 + multiplier;
		if (multiplier >= 15)
			text.fontSize = 28;

		GetComponent<Animator>().SetTrigger ("Bounce");
	}
}
