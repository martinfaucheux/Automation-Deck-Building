using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirectionEnum;

[ExecuteInEditMode]
public class HexGrid : SingletoneBase<HexGrid>
{
    [Header("Grid param")]
    public Vector2Int gridSize;
    public float unitSize = 1f;
    public Vector3 origin;
    [Header("Hex rendering")]
    public GameObject hexPrefab;
    public float lineThickness = 0.2f;

    // Size in world coordinates of the bounding box of the hexGrid
    public Vector2 worldSize
    {
        get => new Vector2(
            (3f / 4f) * hexSize.x * gridSize.x,
            hexSize.y * (gridSize.y + 0.5f)
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
        if (!_colliders.ContainsKey(position))
            _colliders[position] = new Dictionary<HexLayer, HexCollider>();

        _colliders[position][hexCollider.layer] = hexCollider;
    }

    public void RemoveCollider(HexCollider hexCollider)
    {
        _colliders[hexCollider.position].Remove(hexCollider.layer);
    }

    public Vector3 GetWorldPos(Vector2Int position)
    {
        var x = unitSize * 3 / 2 * position.x;
        var y = unitSize * Mathf.Sqrt(3) / 2 * position.y;
        return origin + new Vector3(x, y);
    }

    public Vector2Int GetHexPos(Vector3 worldPosition)
    {
        worldPosition -= origin;

        float x = worldPosition.x / (unitSize * 3 / 2);
        float y = worldPosition.y / (unitSize * Mathf.Sqrt(3) / 2);

        return new Vector2Int((int)x, (int)y);
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
}
