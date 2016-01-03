using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchControlToggle : MonoBehaviour {

	// 0 = TouchScreen, 1 = OnScreenButtons
	public int controlMode;

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
		//Debug.Log ("Click");
		// toggle between touch control modes
		if (controlMode == 0)
		{
			controlMode = 1;
			SettingsManager.instance.ControlSetting = 
				SettingsManager.ControlMode.OnScreenButtons;
		}

		else if (controlMode == 1)
		{
			controlMode = 0;
			SettingsManager.instance.ControlSetting = 
				SettingsManager.ControlMode.TouchScreen;
		}

	}

	public void OnPointerDown()
	{
		if (controlMode == 0)
		{
			image.sprite = onScreenButtonsDown;
		}
		
		else if (controlMode == 1)
		{
			image.sprite = touchScreenDown;
		}
		SoundManager.instance.PlayUISound();
	}

	public void OnPointerUp()
	{
		if (controlMode == 0)
		{
			image.sprite = onScreenButtonsUp;
		}
		
		else if (controlMode == 1)
		{
			image.sprite = touchScreenUp;
		}
	}
}
