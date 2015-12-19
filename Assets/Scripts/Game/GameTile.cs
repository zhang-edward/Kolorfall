using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour {

	private int tileColor;
	public int TileColor{
		get{return tileColor;}
		set{tileColor = value;}
	}

	public Sprite[] tileColors;
	public SpriteRenderer sr;
	public Animator anim;


	void Update()
	{
		UpdateColor();
	}

	protected void UpdateColor()
	{
		if (tileColor == -1)
		{
			sr.enabled = false;
		}
		else
		{
			sr.enabled = true;
			sr.sprite = tileColors[tileColor];
		}
	}
}
