using DirectionEnum;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HexGrid : SingletonBase<HexGrid>
{
    [Header("Grid param")]
    public Vector2Int gridSize;
    public float unitSize = 1f;
    public Vector3 origin;
    [Header("Hex rendering")]
    public GameObject hexPrefab;
    public float lineThickness = 0.2f;
    public Transform hexGridMeshContainer;

    // Size in world coordinates of the bounding box of the hexGrid
    public Vector2 worldSize
    {
        get => new Vector2(
            (0.75f * gridSize.x + 0.25f) * hexSize.x,
            (gridSize.y + 0.5f) * hexSize.y
        );
    }

    private GenericGrid<Dictionary<HexLayer, HexCollider>> _colliders;
    // generated once at runtime
    public Vector2 hexSize { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _colliders = new GenericGrid<Dictionary<HexLayer, HexCollider>>();
        hexSize = unitSize * new Vector2(2f, Mathf.Sqrt(3f));
    }

    public void AddCollider(HexCollider hexCollider)
    {
        Vector2Int position = hexCollider.position;
        SetLayerDict(position);
        _colliders[position][hexCollider.layer] = hexCollider;
    }

    public void RemoveCollider(HexCollider hexCollider)
    {
        _colliders[hexCollider.position].Remove(hexCollider.layer);
    }

    public void MoveCollider(HexCollider hexCollider, Vector2Int position)
    {
        _colliders[hexCollider.position].Remove(hexCollider.layer);
        SetLayerDict(position);
        _colliders[position][hexCollider.layer] = hexCollider;

    }

    public Vector3 GetWorldPos(Vector2Int position, HexLayer hexLayer = HexLayer.FLOOR)
    {
        float x = unitSize * 3 / 2 * position.x;
        float y = unitSize * Mathf.Sqrt(3) / 2 * position.y;
        float z = HexLayerUtil.GetHeight(hexLayer);
        return origin + new Vector3(x, y, z);
    }

    public Vector2Int GetHexPos(Vector3 worldPosition)
    {
        Vector3 worldOffsetPosition = worldPosition - origin;
        float x = worldOffsetPosition.x / (unitSize * 3 / 2);
        float y = worldOffsetPosition.y / (unitSize * Mathf.Sqrt(3) / 2);
        Vector2Int baseHexPos = new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        Vector2Int hexPos = baseHexPos;

        float minDist = Mathf.Infinity;
        for (int xDiff = -1; xDiff <= 1; xDiff++)
        {
            for (int yDiff = -1; yDiff <= 1; yDiff++)
            {
                Vector2Int testPos = baseHexPos + new Vector2Int(xDiff, yDiff);
                if (IsValidCoordinates(testPos))
                {
                    float sqDist = (GetWorldPos(testPos) - worldPosition).sqrMagnitude;
                    if (sqDist < minDist)
                    {
                        minDist = sqDist;
                        hexPos = testPos;
                    }
                }
            }
        }
        return hexPos;
    }


    public HexCollider GetColliderAtPosition(Vector2Int position, HexLayer layer)
    {
        if (_colliders.ContainsKey(position))
            if (_colliders[position].ContainsKey(layer))
                return _colliders[position][layer];
        return null;
    }

    public HexCollider GetNeighbor(Vector2Int position, HexLayer layer, Direction direction)
    {
        return GetColliderAtPosition(position + direction.ToHexPosition(), layer);
    }

    public List<HexCollider> GetNeighbors(Vector2Int position, HexLayer layer)
    {
        List<HexCollider> colliders = new List<HexCollider>();
        foreach (Direction direction in EnumUtil.GetValues<Direction>())
        {
            if (direction == Direction.NONE)
                continue;
            colliders.Add(GetNeighbor(position, layer, direction));
        }
        return colliders;
    }

    public static bool IsValidCoordinates(Vector2Int coordinates) => MathUtil.Modulo(coordinates.x + coordinates.y, 2) == 0;

    private void SetLayerDict(Vector2Int position)
    {
        if (!_colliders.ContainsKey(position))
            _colliders[position] = new Dictionary<HexLayer, HexCollider>();
    }
}
