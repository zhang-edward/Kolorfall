using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static InputManager instance;

	private float startTime;
	private Vector2 startPos;
	private bool couldBeSwipe;
	private float minSwipeDist = 2.0f;
	private float maxSwipeTime = 0.3f;

	public delegate void InputEventWithVector(Vector3 position);
	public event InputEventWithVector onTapDown;

	public delegate void InputEvent();
	public event InputEvent onSwipeDown;
	public event InputEvent onTapLeft;
	public event InputEvent onTapRight;
	public event InputEvent onTapUp;

	private void Awake() {
		if (instance != this)
			instance = this;
	}

	// Update is called once per frame
	void Update() {
		//if (GameManager.instance.mode)
			GetTetrisInput();
		//else
			GetMatchInput();
	}

	private void GetMatchInput() {
		if (Input.GetMouseButton(0)) {
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			onTapDown?.Invoke(worldPoint);
		}
		else if (Input.GetMouseButtonUp(0)) {
			//Debug.Log ("Mouse Up");
			onTapUp?.Invoke();
		}
	}

	private void GetTetrisInput() {
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			onTapLeft?.Invoke();
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			onTapRight?.Invoke();

		if (Input.GetKeyDown(KeyCode.DownArrow))
			onSwipeDown?.Invoke();
#else
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];

			// if touch began
			if (touch.phase == TouchPhase.Began) {
				couldBeSwipe = true;
				startPos = Camera.main.ScreenToWorldPoint(touch.position);
				startTime = Time.time;
			}
			// if touch didn't move, touch is not a swipe
			else if (touch.phase == TouchPhase.Stationary)
				couldBeSwipe = false;
			else if (touch.phase == TouchPhase.Ended) {

				float swipeTime = Time.time - startTime;
				Vector2 curPos = Camera.main.ScreenToWorldPoint(touch.position);
				float swipeDist = (curPos - startPos).magnitude;

				couldBeSwipe = swipeTime < maxSwipeTime && swipeDist > minSwipeDist;
				if (couldBeSwipe) {
					//Debug.Log ("Swipe");
					couldBeSwipe = false;
					if (Mathf.Sign(curPos.y - startPos.y) == -1.0f) {
						onSwipeDown?.Invoke();
					}
				}
				// If not swipe, it is a tap
				else {
					if (curPos.x < 0) // BG of tetrisGameManager has position x of the middle of the board
						onTapLeft?.Invoke();
					else
						onTapRight?.Invoke();
				}
			}
		}
#endif
	}
}
