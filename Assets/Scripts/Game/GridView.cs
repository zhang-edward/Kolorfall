using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
	public Grid grid;
	public GameObject tilePrefab;
	//public SelectMode mode;

	private List<Color> colors = new List<Color>() { Color.red, Color.blue, Color.green, Color.yellow };
	private GameTile[,] tiles;
	private List<GameTile> selected = new List<GameTile>();

	void Start() {
		grid.onGridChanged += ShowGrid;
		grid.onTileEffects += ShowTileEffect;

		// Instantiate and initialize tile objects
		tiles = new GameTile[grid.gridHeight, grid.gridWidth];
		for (int y = 0; y < grid.gridHeight; y++) {
			for (int x = 0; x < grid.gridWidth; x++) {
				tiles[y, x] = Instantiate(tilePrefab, transform).GetComponent<GameTile>();
				tiles[y, x].transform.localPosition = new Vector3(x, y);
			}
		}
	}

	public bool debug;
	public void Update() {
		if (debug) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				for (int y = 0; y < grid.gridHeight; y++) {
					for (int x = 0; x < grid.gridWidth; x++) {
						tiles[y, x].SetColor(Color.white);
					}
				}
			}
		}
	}

	private void ShowTileEffect(TileEffects effects) {
		TileEffects.Effect e = effects.effect;
		foreach (var (x, y, color) in effects.tiles) {
			GameTile tile = tiles[y, x];
			tile.gameObject.SetActive(true);
			switch (e) {
				case TileEffects.Effect.Normal:
					if (grid.GetTile(x, y) != 0) {
						tile.transform.localScale = new Vector3(1f, 1f);
						tile.SetColor(colors[grid.GetTile(x, y) - 1]);
					}
					else
						tile.gameObject.SetActive(false);
					break;
				case TileEffects.Effect.Ghost:
					Color c = colors[color - 1];
					c.a = 0.5f;
					tile.SetColor(c);
					break;
				case TileEffects.Effect.Flash:
					tile.SetColor(Color.white);
					break;
			}
		}
	}

	private void ShowGrid() {
		for (int y = 0; y < grid.gridHeight; y++) {
			for (int x = 0; x < grid.gridWidth; x++) {
				int color = grid.GetTile(x, y);
				tiles[y, x].gameObject.SetActive(color != 0);
				if (color != 0)
					tiles[y, x].SetColor(colors[color - 1]);
			}
		}
	}

	//private void ShowSelected() {
	//	foreach (GameTile tile in selected) {
	//		SelectTile(tile, false);
	//	}
	//	selected.Clear();
	//	foreach ((int, int) coords in grid.selected) {
	//		var (x, y) = coords;
	//		selected.Add(tiles[y, x]);
	//		SelectTile(tiles[y, x], true);
	//	}
	//}

	//private void SelectTile(GameTile tile, bool selected) {
	//	if (selected)
	//		tile.gameObject.SetActive(true);
	//	switch (mode) {
	//		case SelectMode.Ghost:
	//			if (selected) {
	//				Color color = colors[grid.selectedColor - 1];
	//				color.a = 0.5f;
	//				tile.SetColor(color);
	//			}
	//			else {
	//				Color color = colors[grid.selectedColor - 1];
	//				color.a = 0.5f;
	//				tile.SetColor(color);
	//			}
	//			break;
	//		case SelectMode.Shine:
	//			tile.transform.localScale = selected ? new Vector3(1.1f, 1.1f) : new Vector3(1, 1);
	//			break;
	//	}
	//}
}
