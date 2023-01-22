using System.Collections.Generic;

public enum HexLayer
{
    FLOOR = 0,
    BUILDING = 1,
    CONTAINED = 2
}

public static class HexLayerUtil
{
    private static readonly Dictionary<HexLayer, float> _heightMap = new Dictionary<HexLayer, float>
    {
        {HexLayer.FLOOR, 0f },
        {HexLayer.BUILDING, 1f},
        {HexLayer.CONTAINED, 2f}
    };

    public static float GetHeight(HexLayer hexLayer) => -_heightMap[hexLayer];
}