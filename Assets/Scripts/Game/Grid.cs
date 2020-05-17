using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Grid and its subclasses contain grid information and receive inputs that
/// modify the grid.
/// </summary>
public class Grid : MonoBehaviour {

	public int gridHeight;
	public int gridWidth;

	private int[,] grid;

	public delegate void GridEvent();
	public event GridEvent onGridChanged;

	public delegate void TileEffectsEvent(TileEffects effects);
	public event TileEffectsEvent onTileEffects;

	protected void BroadcastTileEffects(TileEffects effects) {
		onTileEffects?.Invoke(effects);
	}

	public void CommitGrid() {
		onGridChanged?.Invoke();
	}

	protected virtual void Awake() {
		grid = new int[gridHeight, gridWidth];
	}

	// ==============================
	// Control
	// ==============================

	/// <summary>
	/// When `RegisterControl()` is called, it can register any input handler functions
	/// </summary>
	public virtual void RegisterControl(InputManager input) {}
	public virtual void ReleaseControl(InputManager input) {}

	// ==============================
	// Grid Reading/Writing
	// ==============================
	public void DeleteTile(int x, int y) {
		grid[y, x] = 0;
	}

	public void SetTile(int x, int y, int color) {
		grid[y, x] = color;
	}

	public void MoveTile(int xs, int ys, int xd, int yd) {
		grid[yd, xd] = grid[ys, xs];
		grid[ys, xs] = 0;
	}

	public int GetTile(int x, int y) {
		return grid[y, x];
	}

	protected bool InBounds(int x, int y) {
		return x < gridWidth && y < gridHeight && x >= 0 && y >= 0;
	}
}
