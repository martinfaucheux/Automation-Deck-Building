using DirectionEnum;
using UnityEngine;

public class HexCollider : MonoBehaviour
{
    [field: SerializeField]
    public Vector2Int position { get; private set; }
    public HexLayer layer;

    public HexGrid grid { get => HexGrid.instance; }

    void Awake()
    {
        SyncPosition();
        grid.AddCollider(this);
    }

    void OnDestroy()
    {
        grid?.RemoveCollider(this);
    }

    public void SetPosition(Vector2Int position)
    {
        grid.MoveCollider(this, position);
        this.position = position;
    }

    public Vector3 GetSnappedPosition()
    {
        Vector3 snappedPosition = grid.GetWorldPos(position, layer);
        return snappedPosition;
    }

    public void SyncPosition()
    {
        position = grid.GetHexPos(this.transform.position);
    }

    public void AlignOnGrid()
    {
        SyncPosition();
        transform.position = GetSnappedPosition();
    }

    public Direction? GetNeighborDirection(HexCollider otherCollider)
    {
        return DirectionUtil.GetDirectionFromPosition(otherCollider.position - this.position);
    }
}
