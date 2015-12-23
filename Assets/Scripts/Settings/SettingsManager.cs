using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour {

	public static SettingsManager instance;

	public int ctrlMode;
	public int curThemeColor;
	public bool mute;

	void Awake()
	{
		// Make this a singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad(gameObject);
	}
}
