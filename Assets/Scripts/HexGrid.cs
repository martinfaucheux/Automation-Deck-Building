using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : SingletoneBase<HexGrid>
{
    public Vector2Int gridSize;
    public float unitSize = 1f;
    public Vector3 origin;
    private Dictionary<Vector2Int, List<HexCollider>> _colliders;
    private Vector2 hexSize;

    protected override void Awake()
    {
        base.Awake();
        _colliders = new Dictionary<Vector2Int, List<HexCollider>>();
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
