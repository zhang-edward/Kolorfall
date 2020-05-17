using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Tetris grid receives grid pieces of type int[,].
/// You can place them in a similar fashion to Tetris-style games.
/// </summary>
public class TetrisGrid : Grid
{
	[SerializeField] private int xStart;
	[SerializeField] private int yStart;

	public delegate void GameEvent();
	public event GameEvent onPiecePlaced;

	private int[,] piece;
	private List<(int, int, int)> ghost = new List<(int, int, int)>();

	protected override void Awake() {
		base.Awake();
	}

	private void Start() {
		CommitGrid();
	}

	public override void RegisterControl(InputManager input) {
		base.RegisterControl(input);
		input.onTapLeft += MoveLeft;
		input.onTapRight += MoveRight;
		input.onSwipeDown += CommitPiece;
	}

	public override void ReleaseControl(InputManager input) {
		base.ReleaseControl(input);
		input.onTapLeft -= MoveLeft;
		input.onTapRight -= MoveRight;
		input.onSwipeDown -= CommitPiece;
	}

	public void ReceivePiece(int[,] piece) {
		this.piece = piece;
		xStart = (gridWidth/2) - (piece.GetLength(1)/2);
		ShowPieceGhost();
	}

	private void ShowPieceGhost() {
		// Get dimensions of piece
		int height = piece.GetLength(0);
		int width = piece.GetLength(1);

		yStart = gridHeight - height;
		while (yStart > 0 && CanFall()) {
			yStart--;
		}

		BroadcastTileEffects(new TileEffects(TileEffects.Effect.Normal, ghost));
		ghost.Clear();
		// Write into ghost piece
		for (int y = yStart; y < yStart + height; y++) {
			for (int x = xStart; x < xStart + width; x++) {
				int color = piece[y - yStart, x - xStart];
				if (color != 0)
					ghost.Add((x, y, color));
			}
		}
		BroadcastTileEffects(new TileEffects(TileEffects.Effect.Ghost, ghost));
	}

	private bool CanFall() {
		int yFall = yStart - 1;
		// Get dimensions of piece
		int height = piece.GetLength(0);
		int width = piece.GetLength(1);
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (piece[y, x] != 0 && GetTile(x + xStart, y + yFall) != 0)
					return false;
			}
		}
		return true;
	}

	private void CommitPiece() {
		foreach (var (x, y, color) in ghost) {
			// Check if we lose
			if (GetTile(x, y) != 0)
				GameManager.instance.Lose();
			SetTile(x, y, color);
		}
		ghost.Clear();
		CheckClearLines();

		// Fire events
		BroadcastTileEffects(new TileEffects(TileEffects.Effect.Normal, ghost));
		onPiecePlaced?.Invoke();
		CommitGrid();
	}

	private void CheckClearLines() {
		for (int y = gridHeight - 1; y >= 0; y--) {
			bool rowClear = true;
			for (int x = 0; x < gridWidth; x++) {
				// if any empty spaces, row is not clear
				if (GetTile(x, y) == 0)
					rowClear = false;
			}
			if (rowClear) {
				ClearLine(y);
			}
		}
	}

	private void ClearLine(int row) {
		for (int y = row; y < gridHeight; y++) {
			for (int x = 0; x < gridWidth; x++) {
				if (y != gridHeight - 1)
					SetTile(x, y, GetTile(x, y + 1));
				else
					SetTile(x, y, 0);
			}
		}
	}

	private void MoveLeft() {
		xStart = Mathf.Max(xStart - 1, 0);
		ShowPieceGhost();
	}


	private void MoveRight() {
		int pieceWidth = piece.GetLength(1);
		xStart = Mathf.Min(xStart + 1, gridWidth - pieceWidth);
		ShowPieceGhost();
	}
}
