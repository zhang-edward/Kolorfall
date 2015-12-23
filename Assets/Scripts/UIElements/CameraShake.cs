using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public Camera cam;
	// how long the shaking lasts
	private float time;
	// the magnitude of the shaking
	private float mag;

	void Update()
	{
		if (time > 0)
		{
			cam.transform.localPosition = new Vector2(transform.position.x, Random.Range (-1.0f, 1.0f) * mag);
			time -= Time.deltaTime;
		}
		else
		{
			cam.transform.localPosition = Vector2.zero;
			time = 0;
		}
	}

	public void Shake(float shakeTime, float shakeMagnitude)
	{
		time = shakeTime;
		mag = shakeMagnitude;
	}
}
