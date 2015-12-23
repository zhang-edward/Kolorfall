using UnityEngine;
using System.Collections;

public class TouchControlSetting : MonoBehaviour {

	// touch control mode
	public enum ControlMode {
		TouchScreen,
		OnScreenButtons
	}
	
	/// <summary>
	/// The controlMode.
	/// </summary>
	public ControlMode ctrlmode;
	
	public GameObject onScreenButtons;
	public TetrisTouchInput tti;
	
	void Update()
	{
		if (ctrlmode == ControlMode.OnScreenButtons)
		{
			onScreenButtons.SetActive(true);
			tti.enabled = false;
			SettingsManager.instance.ctrlMode = 0;
		}
		else if (ctrlmode == ControlMode.TouchScreen)
		{
			onScreenButtons.SetActive(false);
			tti.enabled = true;
			SettingsManager.instance.ctrlMode = 1;
		}
	}
}
