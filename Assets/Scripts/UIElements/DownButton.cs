using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DownButton : MonoBehaviour {

	public TetrisGameManager tgm;

	// if down button is held down for more than 0.7 seconds, quickfall
	public const float maxDownButtonTime = 0.4f;
	private bool downButtonHeld;
	private float downButtonTime;

	public Sprite UpSprite;
	public Sprite DownSprite;

	void Update()
	{
		if (downButtonHeld)
		{
			//Debug.Log ("Down button Held!");
			downButtonTime += Time.deltaTime;
		}
		else
		{
			//Debug.Log ("Down button released!");
			downButtonTime = 0.0f;
		}
		if (downButtonTime >= maxDownButtonTime)
		{
			tgm.QuickFall();
			downButtonTime = 0.0f;
		}
	}

	public void DownButtonDown()
	{
		downButtonHeld = true;
		GetComponent<Image>().sprite = DownSprite;
	}

	public void DownButtonUp()
	{
		downButtonHeld = false;
		GetComponent<Image>().sprite = UpSprite;
		if (downButtonTime < maxDownButtonTime)
			tgm.moveDown();
	}
}
