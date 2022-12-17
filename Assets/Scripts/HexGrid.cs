using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<HexCollider> GetCollidersAtPosition(Vector2Int position) => _colliders[position];

    public Vector3 GetWorldPosition(Vector2Int position)
    {
        int column = position.x;
        int row = position.y;
        float offset = (column % 2) == 0 ? (hexSize.y / 2f) : 0f;

        float horizontalDistance = hexSize.x * (3f / 4f);
        float verticalDistance = hexSize.y;

        float xPosition = column * horizontalDistance;
        float yPosition = row * verticalDistance - offset;

        return origin + new Vector3(xPosition, yPosition, 0f);
    }
}
