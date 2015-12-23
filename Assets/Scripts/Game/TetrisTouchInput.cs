using UnityEngine;
using System.Collections;

public class TetrisTouchInput : MonoBehaviour {

	private TetrisGameManager tgm;

	private float startTime;
	private Vector2 startPos;
	private bool couldBeSwipe;
	private float minSwipeDist = 2.0f;
	private float maxSwipeTime = 0.3f;

	private bool couldBeTap;
	private float maxTapDist = 1.0f;
	private float maxTapTime = 0.5f;
	
/*	private Vector2 startPosX;
	private float xInterval = 3.0f;*/

	void Start()
	{
		tgm = GetComponent<TetrisGameManager>();
/*		StartCoroutine("checkXMovement");*/
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];

			// if touch began
			if (touch.phase == TouchPhase.Began)
			{
				couldBeSwipe = true;
				startPos = Camera.main.ScreenToWorldPoint(touch.position);
				startTime = Time.time;
			}
			// if touch didn't move, touch is not a swipe
			else if (touch.phase == TouchPhase.Stationary)
				couldBeSwipe = false;
			else if (touch.phase == TouchPhase.Ended)
			{
				tgm.fallInterval = 0.5f;

				float swipeTime = Time.time - startTime;
				Vector2 curPos = Camera.main.ScreenToWorldPoint(touch.position);
				float swipeDist = (curPos - startPos).magnitude;

				//Debug.Log ("swipe time: " + swipeTime + "\n" + 
				 //          "swipe dist: " + swipeDist);
				
				couldBeSwipe = swipeTime < maxSwipeTime && swipeDist > minSwipeDist;
				if (couldBeSwipe)
				{
					//Debug.Log ("Swipe");
					couldBeSwipe = false;
					if (Mathf.Sign (curPos.y - startPos.y) == -1.0f)
					{
						if (tgm.HavePiece)
							tgm.QuickFall();
					}
				}
				couldBeTap = swipeTime < maxTapTime && swipeDist < maxTapDist;
				if (couldBeTap)
				{
					couldBeTap = false;
					//Debug.Log ("Tap");
					// check moveDown first
					if (curPos.y < -5)
						tgm.moveDown();
					else
					{
						// 0 is the middle of the screen
						if (curPos.x < 0) // BG of tetrisGameManager has position x of the middle of the board
							tgm.moveLeft();
						else
							tgm.moveRight();
					}
				}
			}
		}
	}
}
