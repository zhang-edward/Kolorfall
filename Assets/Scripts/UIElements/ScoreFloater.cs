using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreFloater : MonoBehaviour {

	Text text;

	// time before fading starts, in seconds
	private float fadeTimer = 0.5f;

	void Awake()
	{
		text = GetComponent<Text>();
	}

	void Update()
	{
		fadeTimer -= Time.deltaTime;
		if (fadeTimer <= 0)
			text.color = new Color(1f, 1f, 1f, text.color.a - 0.05f);

		if (text.color.a <= 0)
			gameObject.SetActive(false);
	}

	public void ResetFade()
	{
		fadeTimer = 1.0f;
		text.color = new Color(1f, 1f, 1f, 1f);
	}
}
