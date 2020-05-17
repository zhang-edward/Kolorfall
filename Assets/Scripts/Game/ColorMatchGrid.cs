using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMatchGrid : Grid
{
	// The view of the next row
	public Grid nextRow;

	// The selected piece that is cleared
	public int[,] piece { get; private set; }

	public delegate void GameEvent();
	public event GameEvent onPieceCleared;

	private List<(int, int, int)> selected;
	private int selectedColor;
	private int turn;

	protected override void Awake() {
		base.Awake();
		selected = new List<(int, int, int)>();
	}

	void Start() {
		InitGrid();
		GenerateNextRow();
	}

	public void InitGrid() {
		for (int y = 0; y < gridHeight / 3; y++)   // board generates only 1/3 of the screen
		{
			for (int x = 0; x < gridWidth; x++) {
				SetTile(x, y, Random.Range(1, GameManager.NUM_COLORS + 1));
			}
		}
		CommitGrid();
	}

	public override void RegisterControl(InputManager input) {
		base.RegisterControl(input);
		input.onTapDown += SelectPosition;
		input.onTapUp += ClearPosition;
	}

	public override void ReleaseControl(InputManager input) {
		base.ReleaseControl(input);
		input.onTapDown -= SelectPosition;
		input.onTapUp -= ClearPosition;
	}

	private Vector2Int WorldToBoardPoint(Vector3 vec) {
		Vector3 ans = vec - transform.position;
		return new Vector2Int(Mathf.RoundToInt(ans.x), Mathf.RoundToInt(ans.y));
	}

	public void SelectPosition(Vector3 pos) {
		Vector2Int selectedPos = WorldToBoardPoint(pos);
		// If selected tile is invalid (out of bounds or empty)
		if (!InBounds(selectedPos.x, selectedPos.y) ||
			GetTile(selectedPos.x, selectedPos.y) == 0) {
			BroadcastTileEffects(new TileEffects(TileEffects.Effect.Normal, selected));
			selected.Clear();
			return;
		}
		// If we are already selecting this tile
		if (selected.Contains((selectedPos.x, selectedPos.y, GetTile(selectedPos.x, selectedPos.y))))
			return;
		// Select a new tile
		else {
			// Populate selected list
			BroadcastTileEffects(new TileEffects(TileEffects.Effect.Normal, selected));
			selected.Clear();
			selectedColor = GetTile(selectedPos.x, selectedPos.y);
			MatchTiles(selectedPos.x, selectedPos.y);

			BroadcastTileEffects(new TileEffects(TileEffects.Effect.Flash, selected));
		}
	}

	public void ClearPosition() {
		if (selected.Count <= 0)
			return;
		// Find bounding box for piece
		int xMin = int.MaxValue;
		int yMin = int.MaxValue;
		int xMax = int.MinValue;
		int yMax = int.MinValue;
		foreach (var (x, y, _) in selected) {
			xMin = Mathf.Min(x, xMin);
			xMax = Mathf.Max(x, xMax);
			yMin = Mathf.Min(y, yMin);
			yMax = Mathf.Max(y, yMax);
		}

		// Remove piece from grid and write to piece variable
		piece = new int[yMax - yMin + 1, xMax - xMin + 1];
		foreach (var(x, y, _) in selected) {
			DeleteTile(x, y);
			piece[y - yMin, x - xMin] = selectedColor;
		}
		Fall();
		// Clear selected list
		IncrementTurn();

		// Fire events
		BroadcastTileEffects(new TileEffects(TileEffects.Effect.Normal, selected));
		selected.Clear();
		onPieceCleared?.Invoke();
		CommitGrid();
	}

	private void IncrementTurn() {
		turn++;
		if (selected.Count == 1) {
			turn = 0;
		}
		if (turn % 3 == 0) {
			AddRow();
		}
	}

	private void AddRow() {
		// Check if we lose
		for (int i = 0; i < gridWidth; i++) {
			if (GetTile(i, gridHeight - 1) != 0)
				GameManager.instance.Lose();
		}

		for (int y = gridHeight - 2; y >= 0; y--) {
			for (int x = 0; x < gridWidth; x++) {
				MoveTile(x, y, x, y + 1);
			}
		}
		// generate new row
		for (int x = 0; x < gridWidth; x++) {
			SetTile(x, 0, nextRow.GetTile(x, 0));
		}
		GenerateNextRow();
	}

	private void GenerateNextRow() {
		for (int i = 0; i < gridWidth; i++)
			nextRow.SetTile(i, 0, Random.Range(1, GameManager.NUM_COLORS + 1));
		nextRow.CommitGrid();
	}

	private void Fall() {
		for (int x = 0; x < gridWidth; x++) {
			FallColumn(x);
		}
	}

	private void FallColumn(int x) {
		Queue<int> col = new Queue<int>();
		for (int i = 0; i < gridHeight; i++) {
			if (GetTile(x, i) != 0) {
				col.Enqueue(GetTile(x, i));
			}
		}
		int y = 0;
		while (col.Count > 0) {
			SetTile(x, y, col.Dequeue());
			y++;
		}
		while (y < gridHeight) {
			SetTile(x, y, 0);
			y++;
		}
	}

	private void MatchTiles(int x, int y) {
		if (!InBounds(x, y) || GetTile(x, y) != selectedColor || selected.Contains((x, y, selectedColor)))
			return;
		selected.Add((x, y, selectedColor));
		MatchTiles(x + 1, y);
		MatchTiles(x - 1, y);
		MatchTiles(x, y + 1);
		MatchTiles(x, y - 1);
	}
}
