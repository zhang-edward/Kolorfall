using System.Collections.Generic;

/// <summary>
/// Describes the effect that will display on a collection of tiles
/// </summary>
public class TileEffects {

	/// <summary>
	/// The effect to play. Some effects will persist, such as Ghost. These need to be
	/// reset back to normal by setting the effect to Normal, or playing an effect which
	/// resets back to normal after being played, such as Flash.
	/// </summary>
	public enum Effect {
		Normal,
		Ghost,
		Flash,
		Drop
	}

	public Effect effect;
	public List<(int, int, int)> tiles;

	public TileEffects(Effect effect) {
		this.effect = effect;
		tiles = new List<(int, int, int)>();
	}

	public TileEffects(Effect effect, List<(int, int, int)> tiles) {
		this.effect = effect;
		this.tiles = tiles;
	}

	public void SetEffect(Effect effect) {
		this.effect = effect;
	}

	public void AddTile(int x, int y, int color) {
		tiles.Add((x, y, color));
	}
}
