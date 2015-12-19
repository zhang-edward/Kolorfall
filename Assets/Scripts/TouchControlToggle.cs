using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchControlToggle : MonoBehaviour {
	
	public SettingsManager.ControlMode controlMode;

	public Sprite touchScreenDown;
	public Sprite touchScreenUp;
	public Sprite onScreenButtonsDown;
	public Sprite onScreenButtonsUp;

	private Image image;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	public void SwitchControlMode()
	{
		Debug.Log ("Click");
		// toggle between touch control modes
		if (controlMode == SettingsManager.ControlMode.TouchScreen)
		{
			controlMode = SettingsManager.ControlMode.OnScreenButtons;
		}

		else if (controlMode == SettingsManager.ControlMode.OnScreenButtons)
		{
			controlMode = SettingsManager.ControlMode.TouchScreen;
		}
		SettingsManager.instance.ctrlmode = controlMode;
	}

	public void OnPointerDown()
	{
		Debug.Log ("Down");
		if (controlMode == SettingsManager.ControlMode.TouchScreen)
		{
			image.sprite = onScreenButtonsDown;
		}
		
		else if (controlMode == SettingsManager.ControlMode.OnScreenButtons)
		{
			image.sprite = touchScreenDown;
		}
	}

	public void OnPointerUp()
	{
		Debug.Log ("Up");
		if (controlMode == SettingsManager.ControlMode.TouchScreen)
		{
			image.sprite = onScreenButtonsUp;
		}
		
		else if (controlMode == SettingsManager.ControlMode.OnScreenButtons)
		{
			image.sprite = touchScreenUp;
		}
	}
}
