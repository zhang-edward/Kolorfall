using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class PlayUISound : MonoBehaviour {

	private Button button;

	void Awake()
	{
		button = this.GetComponent<Button>();
		button.onClick.AddListener(UiSound);
	}

	void UiSound()
	{
		SoundManager.instance.PlayUISound();
	}
}