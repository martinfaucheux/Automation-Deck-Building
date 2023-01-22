using DirectionEnum;
using UnityEngine;

public class HexCollider : MonoBehaviour
{
    public Vector2Int position;
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
