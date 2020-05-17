using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour {

	public SpriteRenderer sr;

	public void SetColor(Color color) {
		sr.color = color;
	}
}
