using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour {

	private Image img;

	void Awake()
	{
		img = GetComponent<Image>();
	}

	void Update()
	{
		Color themeColor = SettingsManager.instance.ThemeColor;
		img.color = new Color(themeColor.r, themeColor.g, themeColor.b);
	}
}
