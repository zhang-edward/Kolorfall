using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThemeController : MonoBehaviour {

	private Image img;

	public Color[] themeColor = new Color[5];
	private int curThemeColor = 0;


	void Awake()
	{
		img = GetComponent<Image>();
	}

	void Update()
	{
		Color bgColor = themeColor[curThemeColor];

		img.color = bgColor;
		Camera.main.backgroundColor = bgColor;
	}

	public void changeThemeLeft()
	{
		if (curThemeColor > 0)
			curThemeColor --;
	}

	public void changeThemeRight()
	{
		if (curThemeColor < themeColor.Length - 1)
			curThemeColor ++;
	}
}
