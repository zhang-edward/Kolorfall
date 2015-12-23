using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThemeSetting : MonoBehaviour {

	// (we can just use built-in button functions, no custom ui script required)
	public Image buttonImage;

	public Color[] themeColor = new Color[5];
	private int curThemeColor = 0;

	void Update()
	{
		Color bgColor = themeColor[curThemeColor];

		buttonImage.color = bgColor;
		Camera.main.backgroundColor = bgColor;

		SettingsManager.instance.curThemeColor = curThemeColor;
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
