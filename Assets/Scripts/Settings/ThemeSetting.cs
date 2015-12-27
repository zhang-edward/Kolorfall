using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThemeSetting : MonoBehaviour {

	// (we can just use built-in button functions, no custom ui script required)
	public Image buttonImage;

	public Color[] themeColor = new Color[5];
	private int themeIndex = 0;

	void Start()
	{
		themeIndex = SettingsManager.instance.ThemeIndex; 
	}

	void Update()
	{
		Color bgColor = themeColor[themeIndex];
		buttonImage.color = bgColor;

		SettingsManager.instance.ThemeIndex = themeIndex;
		SettingsManager.instance.ThemeColor = bgColor;
	}

	public void changeThemeLeft()
	{
		if (themeIndex > 0)
			themeIndex --;
	}

	public void changeThemeRight()
	{
		if (themeIndex < themeColor.Length - 1)
			themeIndex ++;
	}
}
