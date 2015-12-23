using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchControlToggle : MonoBehaviour {

	// Custom UI fuctions to control switching of ctrlMode in TouchControlSetting
	public TouchControlSetting.ControlMode controlMode;
	public TouchControlSetting touchSetting;

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
		if (controlMode == TouchControlSetting.ControlMode.TouchScreen)
		{
			controlMode = TouchControlSetting.ControlMode.OnScreenButtons;
		}

		else if (controlMode == TouchControlSetting.ControlMode.OnScreenButtons)
		{
			controlMode = TouchControlSetting.ControlMode.TouchScreen;
		}
		touchSetting.ctrlmode = controlMode;
	}

	public void OnPointerDown()
	{
		Debug.Log ("Down");
		if (controlMode == TouchControlSetting.ControlMode.TouchScreen)
		{
			image.sprite = onScreenButtonsDown;
		}
		
		else if (controlMode == TouchControlSetting.ControlMode.OnScreenButtons)
		{
			image.sprite = touchScreenDown;
		}
	}

	public void OnPointerUp()
	{
		Debug.Log ("Up");
		if (controlMode == TouchControlSetting.ControlMode.TouchScreen)
		{
			image.sprite = onScreenButtonsUp;
		}
		
		else if (controlMode == TouchControlSetting.ControlMode.OnScreenButtons)
		{
			image.sprite = touchScreenUp;
		}
	}
}
