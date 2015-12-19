using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour {

	public static SettingsManager instance;

	// touch control mode
	public enum ControlMode {
		TouchScreen,
		OnScreenButtons
	}

	public ControlMode ctrlmode;
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
		if (ctrlmode == ControlMode.OnScreenButtons)
		{
			onScreenButtons.SetActive(true);
			tti.enabled = false;

		}
		else if (ctrlmode == ControlMode.TouchScreen)
		{
			onScreenButtons.SetActive(false);
			tti.enabled = true;
		}
	}
}
