using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TileTypes
{
    Water,
    Grass,
    Mountain,
    Snow,
    Sand
}

public class Biomes
{
    public int waterHeight = 6;
    public int landHeightCeiling = 9;
    public int mountainCeiling = 10;

    public TileTypes GetTileTypeFromHeight(float height)
    {
        int height_int = (int) height;

        if (height_int <= waterHeight) { return TileTypes.Water; }
        else if (height_int == waterHeight + 1 ) {return TileTypes.Sand; }
        else if (height_int <= landHeightCeiling) { return TileTypes.Grass; }
        else if (height_int <= mountainCeiling) { return TileTypes.Mountain; }
        else { return TileTypes.Snow; }
    }

    public static Dictionary<TileTypes, Color> ColorMap = new Dictionary<TileTypes, Color>(){
            {TileTypes.Water, Color.blue},
            {TileTypes.Grass, Color.green},
            {TileTypes.Mountain, Color.gray},
            {TileTypes.Snow, Color.white},
            {TileTypes.Sand, Color.yellow}
        };
}
