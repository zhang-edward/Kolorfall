using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour {

	public static SettingsManager instance;

	public enum ControlMode{
		OnScreenButtons,
		TouchScreen
	}

	// Settings
	public ControlMode ControlSetting;
	public Color ThemeColor;
	public int ThemeIndex;
	public bool Mute;
	
	public GameObject onScreenButtons;
	public TetrisTouchInput tti;

	void Awake()
	{
		// Make this a singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (Application.loadedLevelName.Equals("Game"))
		{
			if (ControlSetting == ControlMode.OnScreenButtons)
			{
				onScreenButtons.SetActive(true);
				tti.enabled = false;
			}
			else
			{
				onScreenButtons.SetActive (false);
				tti.enabled = true;
			}
		}
		Camera.main.backgroundColor = ThemeColor;
	}
}
