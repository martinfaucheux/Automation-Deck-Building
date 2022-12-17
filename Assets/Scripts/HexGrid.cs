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

    private GenericGrid<HexCollider> _colliders;
    // generated once at runtime
    public Vector2 hexSize { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _colliders = new GenericGrid<HexCollider>();
        hexSize = unitSize * new Vector2(2f, Mathf.Sqrt(3f));
    }

    public void AddCollider(HexCollider hexCollider)
    {
        Vector2Int position = hexCollider.position;
        if (!_colliders.ContainsKey(position))
            _colliders[position] = new List<HexCollider>();

        _colliders[position].Add(hexCollider);
    }

    public void RemoveCollider(HexCollider hexCollider)
    {
        _colliders[hexCollider.position].Remove(hexCollider);
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

    public List<HexCollider> GetCollidersAtPosition(Vector2Int position)
    {
        if (_colliders.ContainsKey(position))
            return _colliders[position];
        return new List<HexCollider>();
    }

    public List<HexCollider> GetNeighbors(Vector2Int position, Direction direction)
    {
        return GetCollidersAtPosition(position + direction.ToHexPosition());
    }

    public List<HexCollider> GetNeighbors(Vector2Int position)
    {
        List<HexCollider> colliders = new List<HexCollider>();
        foreach (Direction direction in EnumUtil.GetValues<Direction>())
            colliders.AddRange(GetNeighbors(position, direction));

        return colliders;
    }
}
