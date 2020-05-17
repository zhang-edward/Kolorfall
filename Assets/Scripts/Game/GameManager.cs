using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int NUM_COLORS = 4;
    public ColorMatchGrid colorMatch;
    public TetrisGrid tetris;
    public int[,] piece { get; private set; }

    public static GameManager instance;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }

	void Start() {
		colorMatch.RegisterControl(InputManager.instance);
		//tetris.RegisterControl(InputManager.instance);
		//tetris.ReceivePiece(new int[1, 1] { { 1 } });
		//tetris.onPiecePlaced += () => tetris.ReceivePiece(new int[1, 1] { { 1 } });
		colorMatch.onPieceCleared += () => {
			tetris.ReceivePiece(colorMatch.piece);
			SetTetrisControl();
		};
		tetris.onPiecePlaced += SetColorMatchControl;
	}

	void SetColorMatchControl() {
        colorMatch.RegisterControl(InputManager.instance);
        tetris.ReleaseControl(InputManager.instance);
	}

	void SetTetrisControl() {
        tetris.RegisterControl(InputManager.instance);
        colorMatch.ReleaseControl(InputManager.instance);
	}

	public void Lose() {
        print("u lose fool!");
	}
}
